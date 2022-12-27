using app.Services.Scheduler;

namespace app.Services.Extensions;

public static class CronServiceExtension
{
  public static IServiceCollection AddCronJob<T>(this IServiceCollection services, 
    Action<ISchedulerConfiguration<T>> options) where T : CronJobService
  {
    if (options is null)
      throw new ArgumentNullException(nameof(options), @"Please provide scheduler configurations.");

    SchedulerConfiguration<T> configuration = new ();
    options.Invoke(configuration);
    
    if (string.IsNullOrWhiteSpace(configuration.CronExpression))
      throw new ArgumentNullException(nameof(SchedulerConfiguration<T>.CronExpression), @"Empty Cron Expression is not allowed.");

    services.AddSingleton<ISchedulerConfiguration<T>>(configuration);
    services.AddHostedService<T>();

    return services;
  }
}