
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using slack.Config;

namespace slack.Extensions;

public static class SlackServiceExtension
{
  public static IServiceCollection AddSlackService(this IServiceCollection services, Action<SlackConfiguration> options)
  {
    if (services is null) throw new ArgumentNullException(nameof(services));
    if (options is null) throw new ArgumentNullException(nameof(options));

    services
      .Configure(options)
      .AddSingleton(sp => 
        sp.GetRequiredService<IOptions<SlackConfiguration>>().Value);

    services.AddTransient<ISlack, Client.Slack>();

    return services;
  }
}