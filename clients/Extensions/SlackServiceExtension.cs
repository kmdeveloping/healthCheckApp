
using clients.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace clients.Extensions;

public static class SlackServiceExtension
{
  public static IServiceCollection AddSlackService(this IServiceCollection services, Action<SlackClientConfiguration> options)
  {
    if (services is null) throw new ArgumentNullException(nameof(services));
    if (options is null) throw new ArgumentNullException(nameof(options));

    SlackClientConfiguration config = new SlackClientConfiguration();
    options.Invoke(config);
    
    services.AddSingleton(config);
    services.AddTransient<ISlackWebhookClient, SlackWebhookClient>();

    return services;
  }
}