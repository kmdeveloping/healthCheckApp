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
    
    if ((await _client.GetNetworkStatus(cancellationToken)).StatusCode == HttpStatusCode.OK)
      await _slack.SendAsync(SlackMessageEnum.NetworkStatusRestored, "Network Up");
  }

  public override Task StopAsync(CancellationToken cancellationToken)
  {
    _logger.LogInformation("Network Scan Completed {Time}", DateTime.Now);
    return base.StopAsync(cancellationToken);
  }
}