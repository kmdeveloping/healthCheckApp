namespace clients.Configuration;

public interface IClientConfiguration<T>
{
  string DestinationUri { get; set; }
  string? AccessToken { get; set; }
}

public class ClientConfiguration<T> : IClientConfiguration<T>
{
  public string DestinationUri { get; set; }
  public string? AccessToken { get; set; }
}