using System.Text.Json;
using app.Services.Exceptions;
using Microsoft.Extensions.Caching.Distributed;
using Serilog;
using StackExchange.Redis;
using ILogger = Serilog.ILogger;

namespace app.Services;

public static class CacheServiceProvider
{
  private static readonly ILogger Logger = Log.ForContext(typeof(CacheServiceProvider));

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
          Logger.Error("{Message}", ex.Message);
          throw;
        default:
          Logger.Error(ex, "{DateTime} - Error in {Method}", DateTime.Now, nameof(GetCacheRecordAsync));
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
          Logger.Error(ex, "{DateTime} - Error in {Method}", DateTime.Now, nameof(SetCacheRecordAsync));
          throw;
        default:
          Logger.Error(ex, "{DateTime} - Error in {Method}", DateTime.Now, nameof(SetCacheRecordAsync));
          break;
      }
    }
  }
}