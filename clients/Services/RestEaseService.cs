using Microsoft.Extensions.Logging;

namespace clients.Services;

public interface IRestEaseService
{
}

public abstract class RestEaseService : IRestEaseService
{
  private readonly ILogger _logger;
  private string _clientUri;
  private string? _accessToken;
  
  protected RestEaseService(ILogger logger, string clientUri, string? accessToken = null)
  {
    _logger = logger;
    _clientUri = clientUri;
    _accessToken = accessToken;
  }
}