using System.Text.Json;
using Microsoft.Extensions.Logging;
using RestEase;

namespace clients;

public interface INetworkScanningClient
{
  /// <summary>
  /// Scan home network for uptime monitoring
  /// </summary>
  /// <param name="cancellationToken"></param>
  /// <returns></returns>
  [Get]
  Task<HttpResponseMessage> GetNetworkStatus(CancellationToken cancellationToken = default);
}

public class NetworkScanningClient : INetworkScanningClient
{
  private readonly ILogger<NetworkScanningClient> _logger;
  private readonly INetworkScanningClient _decorator;

  public NetworkScanningClient(INetworkScanningClient decorator, ILogger<NetworkScanningClient> logger)
  {
    _logger = logger;
    _decorator = decorator;
  }

  public async Task<HttpResponseMessage> GetNetworkStatus(CancellationToken cancellationToken = default)
  {
    try
    {
      var response = await _decorator.GetNetworkStatus(cancellationToken);
      _logger.LogDebug("Response: {Response}", JsonSerializer.Serialize(response));
      return response;
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Message: {Message}", ex.Message);
      throw;
    }
  }
}