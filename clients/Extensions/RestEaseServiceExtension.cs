using clients.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RestEase.HttpClientFactory;

namespace clients.Extensions;

public static class RestEaseServiceExtension
{
  public static IServiceCollection AddRestClient<TInterface, TClass>(this IServiceCollection services, Action<IClientConfiguration<TClass>> options) 
    where TInterface : class where TClass : TInterface
  {
    if (services is null) throw new ArgumentNullException(nameof(services));
    if (options is null) throw new ArgumentNullException(nameof(options));

    ClientConfiguration<TClass> config = new ();
    options.Invoke(config);
    
    services.AddSingleton<IClientConfiguration<TClass>>(config);
    services.AddRestEaseClient<TInterface>(config.DestinationUri);
    services.Decorate(typeof(TInterface), typeof(TClass));

    return services;
  }
}