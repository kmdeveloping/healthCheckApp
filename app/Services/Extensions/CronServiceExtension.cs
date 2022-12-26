using app.Scheduler;

namespace app.Services.Extensions;

public static class CronServiceExtension
{
  public static IServiceCollection AddCronJob<T>(this IServiceCollection services, Action<ISchedulerConfig<T>> options)
    where T : CronJobService
  {
    if (options is null)
      throw new ArgumentNullException(nameof(options), @"Please provide scheduler configurations.");

    SchedulerConfig<T> config = new ();
    options.Invoke(config);
    
    if (string.IsNullOrWhiteSpace(config.CronExpression))
      throw new ArgumentNullException(nameof(SchedulerConfig<T>.CronExpression), @"Empty Cron Expression is not allowed.");

    services.AddSingleton<ISchedulerConfig<T>>(config);
    services.AddHostedService<T>();

    return services;
  }
}