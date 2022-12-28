using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;

namespace app.Services;

public static class DataCacheService
{
  public static async Task<T?> GetCacheRecordAsync<T>(this IDistributedCache cache, string recordId)
  {
    try
    {
      var record = await cache.GetStringAsync(recordId);
      return !string.IsNullOrEmpty(record) ? JsonSerializer.Deserialize<T>(record) : default;
    }
    catch (Exception ex)
    {
      return default;
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
      // ignored
    }
  }

}