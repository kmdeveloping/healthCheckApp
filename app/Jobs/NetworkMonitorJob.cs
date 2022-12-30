using System.Net;
using app.Services;
using app.Services.Exceptions;
using app.Services.Scheduler;
using clients;
using clients.Models;
using Microsoft.Extensions.Caching.Distributed;
using StackExchange.Redis;

namespace app.Jobs;

public class NetworkMonitorJob : CronJobService
{
  private readonly ILogger<NetworkMonitorJob> _logger;
  private readonly ISlackWebhookClient _slack;
  private readonly INetworkScanningClient _client;
  private readonly IDistributedCache _cache;
  private const string RedisKey = "net_state";

  public NetworkMonitorJob(ISchedulerConfiguration<NetworkMonitorJob> configuration, 
    ILogger<NetworkMonitorJob> logger, 
    ISlackWebhookClient slack, 
    INetworkScanningClient client,
    IDistributedCache cache) : base(configuration.CronExpression, configuration.TimeZoneInfo)
  {
    _logger = logger;
    _slack = slack;
    _client = client;
    _cache = cache;
  }
  
  public override Task StartAsync(CancellationToken cancellationToken)
  {
    _logger.LogInformation("Starting Network Scan {Time}", DateTime.Now);
    return base.StartAsync(cancellationToken);
  }

  public override async Task Execute(CancellationToken cancellationToken)
  {
    _logger.LogInformation("{JobName} started at {Time}", nameof(NetworkMonitorJob), DateTime.Now);

    try
    {
      HttpStatusCode status = (await _client.GetNetworkStatus(cancellationToken)).StatusCode;

      if (await CheckNetworkForPreviousErrorState(status))
      {
        await _slack.SendAsync(SlackMessageEnum.NetworkStatusRestored, "Previous Error State Cleared");
        await _cache.SetCacheRecordAsync(RedisKey, status);
      }
      else if (CheckUnknownNetworkStates(status))
      {
        await _slack.SendAsync(SlackMessageEnum.NetworkStatusError,
          $"Unexpected response from scan {status.ToString()}");
        await _cache.SetCacheRecordAsync(RedisKey, status);
      }
      else
      {
        _logger.LogInformation("No changes to network state");
        await _cache.SetCacheRecordAsync(RedisKey, status);
      }
    }
    catch (HttpRequestException ex)
    {
      // only send one notification to slack
      if (await _cache.GetCacheRecordAsync<HttpStatusCode>(RedisKey) != ex.StatusCode)
      {
        await _slack.SendAsync(SlackMessageEnum.NetworkStatusError, ex.Message);
        
        if (ex.StatusCode is null)
          await _cache.SetCacheRecordAsync(RedisKey, HttpStatusCode.InternalServerError);
        else
          await _cache.SetCacheRecordAsync(RedisKey, ex.StatusCode);
      }
      else
      {
        await _cache.SetCacheRecordAsync(RedisKey, ex.StatusCode);
      }
    }
    catch (Exception ex)
    {
      switch (ex)
      {
        case DataCacheNotFoundException:
          _logger.LogInformation("Message: {ExceptionMessage}, Setting cache initial value {DateTime}", ex.Message,
            DateTime.Now);
          await _cache.SetCacheRecordAsync(RedisKey, HttpStatusCode.OK);
          break;
        case RedisConnectionException:
        case RedisTimeoutException:
          await _slack.SendAsync(SlackMessageEnum.RedisClientError, ex.Message);
          break;
        default:
          await _slack.SendAsync(SlackMessageEnum.UnknownError, ex.Message);
          break;
      }
    }
  }

  public override Task StopAsync(CancellationToken cancellationToken)
  {
    _logger.LogInformation("{JobName} stopped at {Time}", nameof(NetworkMonitorJob), DateTime.Now);
    return base.StopAsync(cancellationToken);
  }

  private async Task<bool> CheckNetworkForPreviousErrorState(HttpStatusCode currentStatusCode)
  {
    HttpStatusCode? record = await _cache.GetCacheRecordAsync<HttpStatusCode>(RedisKey);

    _logger.LogInformation("Previous cached data: {Record}", record.ToString());

    return record != HttpStatusCode.OK && record != 0 && currentStatusCode == HttpStatusCode.OK;
  }

  private bool CheckUnknownNetworkStates(HttpStatusCode currentStatusCode)
  { 
    return currentStatusCode is >= HttpStatusCode.MultipleChoices and < HttpStatusCode.InternalServerError;
  }
}