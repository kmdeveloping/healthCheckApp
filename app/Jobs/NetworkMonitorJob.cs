using app.Services;
using app.Services.Scheduler;

namespace app.Jobs;

public class NetworkMonitorJob : CronJobService
{
  private readonly ILogger<NetworkMonitorJob> _logger;

  public NetworkMonitorJob(ISchedulerConfig<NetworkMonitorJob> config, ILogger<NetworkMonitorJob> logger) : 
    base(config.CronExpression, config.TimeZoneInfo)
  {
    _logger = logger;
  }
  
  public override Task StartAsync(CancellationToken cancellationToken)
  {
    _logger.LogInformation("Starting Network Scan {Time}", DateTime.Now);
    return base.StartAsync(cancellationToken);
  }

  public override async Task Execute(CancellationToken cancellationToken)
  {
    _logger.LogInformation("Network Scan running {Time}", DateTime.Now);
  }

  public override Task StopAsync(CancellationToken cancellationToken)
  {
    _logger.LogInformation("Network Scan Completed {Time}", DateTime.Now);
    return base.StopAsync(cancellationToken);
  }
}