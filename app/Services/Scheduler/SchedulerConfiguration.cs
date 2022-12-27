namespace app.Services.Scheduler;

public interface ISchedulerConfiguration<T>
{
  string CronExpression { get; set; }
  TimeZoneInfo TimeZoneInfo { get; set; }
}

public class SchedulerConfiguration<T> : ISchedulerConfiguration<T>
{
  public string CronExpression { get; set; }
  public TimeZoneInfo TimeZoneInfo { get; set; }
}