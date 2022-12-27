using clients.Configuration;
using clients.Services;
using Microsoft.Extensions.DependencyInjection;
using RestEase.HttpClientFactory;

namespace clients.Extensions;

public static class RestEaseServiceExtension
{
  public static IServiceCollection AddRestEaseServices<TInterface, TClass>(this IServiceCollection services, Action<IRestEaseClientConfiguration<TInterface>> options) 
    where TInterface : class, IRestEaseService where TClass : RestEaseService, TInterface
  {
    if (options is null)
      throw new ArgumentNullException(nameof(options), @"Please provide client uri configurations.");

    RestEaseClientConfiguration<TInterface> clientConfiguration = new();
    options.Invoke(clientConfiguration);

    if (string.IsNullOrWhiteSpace(clientConfiguration.ClientUri))
      throw new ArgumentNullException(nameof(RestEaseClientConfiguration<TInterface>.ClientUri), @"Client uri required for setup");

    services
      .AddSingleton<IRestEaseClientConfiguration<TInterface>>(clientConfiguration)
      //.Decorate(typeof(TInterface), typeof(TClass))
      .AddRestEaseClient<TInterface>(clientConfiguration.ClientUri);

    return services;
  }
}