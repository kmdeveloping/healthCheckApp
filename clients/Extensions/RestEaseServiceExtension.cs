using clients.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RestEase.HttpClientFactory;

namespace clients.Extensions;

public static class RestEaseServiceExtension
{
  public static IServiceCollection AddRestEaseClientServices<TInterface, TClass>(this IServiceCollection services, Action<RestEaseClientConfiguration> options) 
    where TInterface : class where TClass : TInterface
  {
    if (services is null) throw new ArgumentNullException(nameof(services));
    if (options is null) throw new ArgumentNullException(nameof(options));

    RestEaseClientConfiguration config = new RestEaseClientConfiguration();
    options.Invoke(config);
    
    services.AddRestEaseClient<TInterface>(config.ClientUri);
    services.Decorate(typeof(TInterface), typeof(TClass));

    return services;
  }
}