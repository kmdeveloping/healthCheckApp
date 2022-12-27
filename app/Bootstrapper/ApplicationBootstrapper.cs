using app.Jobs.Extensions;
using slack.Extensions;

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

    services.AddScheduledJobs();

    return builder.Build();
  }
}