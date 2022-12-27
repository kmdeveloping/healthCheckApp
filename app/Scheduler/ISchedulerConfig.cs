using System;

namespace app.Scheduler;

public interface ISchedulerConfig<T>
{
  string CronExpression { get; set; }
  TimeZoneInfo TimeZoneInfo { get; set; }
}