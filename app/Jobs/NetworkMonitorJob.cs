using app.Services;
using app.Services.Scheduler;
using clients;

namespace app.Jobs;

public class NetworkMonitorJob : CronJobService
{
  private readonly ILogger<NetworkMonitorJob> _logger;
  private readonly INetworkScanningClient _client;

  public NetworkMonitorJob(ISchedulerConfiguration<NetworkMonitorJob> configuration, 
    ILogger<NetworkMonitorJob> logger, INetworkScanningClient client) : base(configuration.CronExpression, configuration.TimeZoneInfo)
  {
    _logger = logger;
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
    
    var status = await _client.GetNetworkStatus(cancellationToken);

    // todo add logic methods for non 200 status code handling and for network restored handling
    _logger.LogInformation("Status: {Status}",status.StatusCode.ToString());
  }

  public override Task StopAsync(CancellationToken cancellationToken)
  {
    _logger.LogInformation("Network Scan Completed {Time}", DateTime.Now);
    return base.StopAsync(cancellationToken);
  }
}