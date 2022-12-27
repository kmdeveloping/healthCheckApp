using app.Services.Extensions;

namespace app.Jobs.Extensions;

public static class JobsServiceExtension
{
  public static IServiceCollection AddScheduledJobs(this IServiceCollection services)
  {
    if (services is null) throw new ArgumentNullException(nameof(services));

    services.AddCronJob<NetworkMonitorJob>(opt =>
    {
      opt.CronExpression = @"*/5 * * * * *";
      opt.TimeZoneInfo = TimeZoneInfo.Local;
    });
    
    services.AddCronJob<TestJob1>(opt =>
    {
      opt.CronExpression = @"*/5 * * * *";
      opt.TimeZoneInfo = TimeZoneInfo.Local;
    });
    
    return services;
  }
}