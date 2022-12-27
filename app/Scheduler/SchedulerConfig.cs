using System;

namespace app.Scheduler;

public class SchedulerConfig<T> : ISchedulerConfig<T>
{
  public string CronExpression { get; set; }
  public TimeZoneInfo TimeZoneInfo { get; set; }
}