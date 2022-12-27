using Microsoft.Extensions.DependencyInjection;

namespace clients.Extensions;

public static class ClientExtension
{
  public static IServiceCollection AddRestEaseClients(this IServiceCollection services)
  {
    if (services is null) throw new ArgumentNullException(nameof(services));
    
    services.AddRestEaseServices<INetworkScanningClient>(opt =>
    {
      opt.ClientUri = "https://kmcloud.co";
    });
    
    return services;
  }
}