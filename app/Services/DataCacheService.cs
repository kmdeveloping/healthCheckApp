using System.Text.Json;
using app.Services.Exceptions;
using Microsoft.Extensions.Caching.Distributed;
using Serilog;
using StackExchange.Redis;
using ILogger = Serilog.ILogger;

namespace app.Services;

public static class DataCacheService
{
  private static readonly ILogger log = Log.ForContext(typeof(DataCacheService));
  
  public static async Task<T?> GetCacheRecordAsync<T>(this IDistributedCache cache, string recordId)
  {
    try
    {
      var record = await cache.GetStringAsync(recordId);
      return !string.IsNullOrEmpty(record) ? JsonSerializer.Deserialize<T>(record) : 
        throw new DataCacheNotFoundException("No cache record found, please set a new one.");
    }
    catch (Exception ex)
    {
      switch (ex)
      {
        case RedisTimeoutException:
        case RedisConnectionException:
          log.Error("{Message}", ex.Message);
          throw;
        default:
          log.Error(ex, "{DateTime} - Error in {Method}", DateTime.Now, nameof(GetCacheRecordAsync));
          throw;
      }
    }
  }
  
  public static async Task SetCacheRecordAsync<T>(this IDistributedCache cache, string recordId, T data, TimeSpan? absoluteExpireTime = null, TimeSpan? slidingExpireTime = null)
  {
    var expireTimeOptions = new DistributedCacheEntryOptions
    {
      AbsoluteExpirationRelativeToNow = absoluteExpireTime ?? TimeSpan.FromMinutes(30),
      SlidingExpiration = slidingExpireTime
    };

    try
    {
      await cache.SetStringAsync(recordId, JsonSerializer.Serialize(data), expireTimeOptions);
    }
    catch (Exception ex)
    {
      switch (ex)
      {
        case RedisTimeoutException:
        case RedisConnectionException:
          log.Error(ex, "{DateTime} - Error in {Method}", DateTime.Now, nameof(SetCacheRecordAsync));
          throw;
        default:
          log.Error(ex, "{DateTime} - Error in {Method}", DateTime.Now, nameof(SetCacheRecordAsync));
          break;
      }
    }
  }

}