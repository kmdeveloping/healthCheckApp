using System.Net;
using app.Services;
using app.Services.Scheduler;
using clients;
using clients.Models;

namespace app.Jobs;

public class NetworkMonitorJob : CronJobService
{
  private readonly ILogger<NetworkMonitorJob> _logger;
  private readonly ISlackWebhookClient _slack;
  private readonly INetworkScanningClient _client;

  public NetworkMonitorJob(ISchedulerConfiguration<NetworkMonitorJob> configuration, 
    ILogger<NetworkMonitorJob> logger, 
    ISlackWebhookClient slack, 
    INetworkScanningClient client) : base(configuration.CronExpression, configuration.TimeZoneInfo)
  {
    _logger = logger;
    _slack = slack;
    _client = client;
  }
  
  public override Task StartAsync(CancellationToken cancellationToken)
  {
    _logger.LogInformation("Starting Network Scan {Time}", DateTime.Now);
    return base.StartAsync(cancellationToken);
  }

  public override async Task Execute(CancellationToken cancellationToken)
  {
    _logger.LogInformation("Network Scan running {Time}", DateTime.Now);

    try
    {
      // todo add getPreviousStatusFromRedis method for the networkRestored state
      bool networkWasPreviouslyInErrorState = true;

      var status = await _client.GetNetworkStatus(cancellationToken);

      if (networkWasPreviouslyInErrorState)
      {
        await _slack.SendAsync(SlackMessageEnum.NetworkStatusRestored, "Previous Error State Cleared");
      }
      
      // todo add database update with current state 
      var statusType = status.StatusCode.ToString();
      Console.WriteLine(statusType);
    }
    catch (Exception ex)
    {
      // todo add exception for Redis Client Error and HttpResponse error since they are different messages
      await _slack.SendAsync(SlackMessageEnum.NetworkStatusError, ex.Message); 
    }
  }

  public override Task StopAsync(CancellationToken cancellationToken)
  {
    _logger.LogInformation("Network Scan Completed {Time}", DateTime.Now);
    return base.StopAsync(cancellationToken);
  }
}