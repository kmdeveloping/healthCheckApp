
using clients.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace clients.Extensions;

public static class SlackServiceExtension
{
  public static IServiceCollection AddSlackService(this IServiceCollection services, Action<SlackClientConfiguration> options)
  {
    if (services is null) throw new ArgumentNullException(nameof(services));
    if (options is null) throw new ArgumentNullException(nameof(options));

    services.Configure(options)
      .AddSingleton(sp => sp.GetRequiredService<IOptions<SlackClientConfiguration>>().Value)
      .AddTransient<ISlackWebhookClient, SlackWebhookClient>();

    return services;
  }
}