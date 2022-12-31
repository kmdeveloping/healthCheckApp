using app.Jobs;
using app.Services;
using app.Services.Extensions;
using clients;
using Serilog;
using clients.Extensions;

namespace app.Bootstrapper;

public static class ApplicationBootstrapper
{
  public static WebApplication BuildServices(this WebApplicationBuilder builder)
  {
    IServiceCollection services = builder.Services;
    ConfigurationManager configuration = builder.Configuration;

    builder.Host.UseSerilog((ctx, lc) =>
      lc.ReadFrom.Configuration(ctx.Configuration).WriteTo.Console());
    
    services.AddSlackService(opt =>
    {
      opt.WebhookUrl = configuration["SlackConfiguration:WebhookUrl"];
    });

    services.AddRestEaseClientServices<INetworkScanningClient, NetworkScanningClient>(opt => 
      opt.ClientUri = configuration["DefaultScanUrl"]);

    services.AddStackExchangeRedisCache(opt =>
    {
      opt.Configuration = configuration.GetConnectionString("RedisConnection");
      opt.InstanceName = "net_monitor_";
    });

    services.AddCronJob<NetworkMonitorJob>(opt =>
    {
      opt.CronExpression = configuration["CronConfiguration:DefaultCron"];
      opt.TimeZoneInfo = TimeZoneInfo.Local;
    });

    return builder.Build();
  }
}