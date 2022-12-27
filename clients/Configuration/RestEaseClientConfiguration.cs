namespace clients.Configuration;

public interface IRestEaseClientConfiguration<T>
{
  string ClientUri { get; set; }
  string? AccessToken { get; set; }
}

public class RestEaseClientConfiguration<T> : IRestEaseClientConfiguration<T>
{
  public string ClientUri { get; set; }
  public string? AccessToken { get; set; }
}