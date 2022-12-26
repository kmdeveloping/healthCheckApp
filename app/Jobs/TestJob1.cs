using System.Text.Json;
using app.Scheduler;
using app.Services;

namespace app.Jobs;

public class TestJob1 : CronJobService
{
  private readonly ILogger<TestJob1> _logger;

  public TestJob1(ISchedulerConfig<TestJob1> config, ILogger<TestJob1> logger) :
    base(config.CronExpression, config.TimeZoneInfo)
  {
    _logger = logger;
  }

  public override Task StartAsync(CancellationToken cancellationToken)
  {
    _logger.LogInformation("Starting Test Job 1");
    return base.StartAsync(cancellationToken);
  }

  protected override Task Execute(CancellationToken cancellationToken)
  {
    _logger.LogInformation("{Now} Test job is running", DateTime.Now);
    Console.WriteLine(JsonSerializer.Serialize(CountToTen()));
    return Task.CompletedTask;
  }

  public override Task StopAsync(CancellationToken cancellationToken)
  {
    _logger.LogInformation("Test job 1 is stopping.");
    return base.StopAsync(cancellationToken);
  }

  private List<int> CountToTen()
  {
    List<int> response = new();
    var i = 0;
    for (; i <= 100; i++)
    {
      response.Add(i);
    }

    return response;
  }
}