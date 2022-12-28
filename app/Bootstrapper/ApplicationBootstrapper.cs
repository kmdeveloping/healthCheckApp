using app.Jobs;
using app.Services.Extensions;
using clients.Extensions;

namespace app.Bootstrapper;

public static class ApplicationBootstrapper
{
  public static WebApplication BuildServices(this WebApplicationBuilder builder)
  {
    
    var services = builder.Services;
    var configuration = builder.Configuration;
    
    services.AddSlackService(opt =>
    {
      opt.WebhookUrl = configuration["SlackConfiguration:WebhookUrl"];
    });

    services.AddRestEaseClients();
    
    services.AddCronJob<NetworkMonitorJob>(opt =>
    {
      opt.CronExpression = @"* * * * *";
      opt.TimeZoneInfo = TimeZoneInfo.Local;
    });

    services.AddStackExchangeRedisCache(opt =>
    {
      opt.Configuration = configuration.GetConnectionString("RedisConnection");
      opt.InstanceName = "net_monitor_";
    });

    return builder.Build();
  }
}