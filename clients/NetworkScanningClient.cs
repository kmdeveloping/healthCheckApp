using clients.Configuration;
using clients.Services;
using Microsoft.Extensions.Logging;
using RestEase;

namespace clients;

public interface INetworkScanningClient : IRestEaseService
{
  /// <summary>
  /// Scan home network for uptime monitoring
  /// </summary>
  /// <returns></returns>
  [Get]
  Task<HttpResponseMessage> GetNetworkStatus(CancellationToken cancellationToken = default);
}

public class NetworkScanningClient : RestEaseService, INetworkScanningClient
{
  private readonly ILogger<NetworkScanningClient> _logger;
  private readonly INetworkScanningClient _decorator;

  public NetworkScanningClient(IRestEaseClientConfiguration<NetworkScanningClient> clientConfiguration, INetworkScanningClient decorator, ILogger<NetworkScanningClient> logger) : 
    base(clientConfiguration.ClientUri, clientConfiguration.AccessToken)
  {
    _logger = logger;
    _decorator = decorator;
  }

  public async Task<HttpResponseMessage> GetNetworkStatus(CancellationToken cancellationToken = default)
  {
    try
    {
      return await _decorator.GetNetworkStatus(cancellationToken);
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Message: {Message}", ex.Message);
      throw;
    }
  }
}