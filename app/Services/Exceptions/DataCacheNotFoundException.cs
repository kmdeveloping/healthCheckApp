namespace app.Services.Exceptions;

public class DataCacheNotFoundException : Exception
{
  public DataCacheNotFoundException(string message) : base(message)
  {
  }

  public DataCacheNotFoundException(string message, Exception innerException) : base(message, innerException)
  {
  }
}