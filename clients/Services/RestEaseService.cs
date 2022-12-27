using Microsoft.Extensions.Logging;

namespace clients.Services;

public interface IRestEaseService
{
}

public abstract class RestEaseService : IRestEaseService
{
  private string _clientUri;
  private string? _accessToken;
  
  protected RestEaseService(string clientUri, string? accessToken = null)
  {
    _clientUri = clientUri;
    _accessToken = accessToken;
  }
}