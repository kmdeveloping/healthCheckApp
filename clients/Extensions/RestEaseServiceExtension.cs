using clients.Configuration;
using clients.Services;
using Microsoft.Extensions.DependencyInjection;
using RestEase.HttpClientFactory;

namespace clients.Extensions;

public static class RestEaseServiceExtension
{
  public static IServiceCollection AddRestEaseServices<T>(this IServiceCollection services, 
    Action<IRestEaseClientConfiguration<T>> options) where T : class, IRestEaseService
  {
    if (options is null)
      throw new ArgumentNullException(nameof(options), @"Please provide client uri configurations.");

    RestEaseClientConfiguration<T> clientConfiguration = new();
    options.Invoke(clientConfiguration);

    if (string.IsNullOrWhiteSpace(clientConfiguration.ClientUri))
      throw new ArgumentNullException(nameof(RestEaseClientConfiguration<T>.ClientUri), @"Client uri required for setup");

    services.AddSingleton<IRestEaseClientConfiguration<T>>(clientConfiguration);
    services.AddRestEaseClient<T>(clientConfiguration.ClientUri);

    return services;
  }
}