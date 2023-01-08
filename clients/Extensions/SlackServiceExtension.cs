
using clients.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace clients.Extensions;

public static class SlackServiceExtension
{
  public static IServiceCollection AddSlackClient<TI, TC>(this IServiceCollection services, Action<IClientConfiguration<TC>> options) 
    where TI : class where TC : TI
  {
    if (services is null) throw new ArgumentNullException(nameof(services));
    if (options is null) throw new ArgumentNullException(nameof(options));

    ClientConfiguration<TC> config = new ();
    options.Invoke(config);
    
    services.AddSingleton<IClientConfiguration<TC>>(config);
    services.AddTransient(typeof(TI), typeof(TC));

    return services;
  }
}