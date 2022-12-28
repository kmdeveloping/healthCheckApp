using app.Jobs;
using app.Services.Extensions;
using clients;
using clients.Extensions;

namespace app.Bootstrapper;

public static class ApplicationBootstrapper
{
  public static WebApplication BuildServices(this WebApplicationBuilder builder)
  {
    IServiceCollection services = builder.Services;
    ConfigurationManager configuration = builder.Configuration;
    
    services.AddSlackService(opt =>
    {
      opt.WebhookUrl = configuration["SlackConfiguration:WebhookUrl"];
    });

    services.AddStackExchangeRedisCache(opt =>
    {
      opt.Configuration = configuration.GetConnectionString("RedisConnection");
      opt.InstanceName = "net_monitor_";
    });
    
    services.AddRestEaseServices<INetworkScanningClient, NetworkScanningClient>(opt =>
    {
      opt.ClientUri = configuration["DefaultScanUrl"];
    });
    
    services.AddCronJob<NetworkMonitorJob>(opt =>
    {
      opt.CronExpression = configuration["CronConfiguration:DefaultCron"];
      opt.TimeZoneInfo = TimeZoneInfo.Local;
    });

    return builder.Build();
  }
}