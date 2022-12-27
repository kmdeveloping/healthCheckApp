using Cronos;
using Timer = System.Timers.Timer;

namespace app.Services;

public abstract class CronJobService : IHostedService, IDisposable
{
  private Timer? _timer;
  private readonly CronExpression _expression;
  private readonly TimeZoneInfo _timeZoneInfo;

  protected CronJobService(string cronExpression, TimeZoneInfo timeZoneInfo)
  {
    _expression = CronExpression.Parse(cronExpression, CronFormat.IncludeSeconds);
    _timeZoneInfo = timeZoneInfo;
  }

  private async Task ScheduleJob(CancellationToken cancellationToken)
  {
    DateTimeOffset? nextJob = _expression.GetNextOccurrence(DateTimeOffset.Now, _timeZoneInfo);

    if (nextJob.HasValue)
    {
      TimeSpan delay = nextJob.Value - DateTimeOffset.Now;
      if (delay.TotalMilliseconds <= 0)
      {
        await ScheduleJob(cancellationToken);
      }

      _timer = new Timer(delay.TotalMilliseconds);
      _timer.Elapsed += async (sender, args) =>
      {
        _timer.Dispose();
        _timer = null;

        if (!cancellationToken.IsCancellationRequested)
        {
          await Execute(cancellationToken);
        }

        if (!cancellationToken.IsCancellationRequested)
        {
          await ScheduleJob(cancellationToken);  
        }
      };
      
      _timer.Start();
    }

    await Task.CompletedTask;
  }
  
  public virtual async Task StartAsync(CancellationToken cancellationToken)
  {
    await ScheduleJob(cancellationToken);
  }

  public virtual async Task StopAsync(CancellationToken cancellationToken)
  {
    _timer.Stop();
    await Task.CompletedTask;
  }

  public virtual void Dispose()
  {
    _timer.Dispose();
  }

  public virtual async Task Execute(CancellationToken cancellationToken)
  {
    await Task.Delay(5000, cancellationToken);
  }
}