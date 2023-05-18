using Microsoft.Extensions.Caching.Memory;

namespace Kirel.Identity.Core.CacheManager;

/// <summary>
/// 
/// </summary>
public class KirelCacheManager
{
    private readonly IMemoryCache _cache;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="cache"></param>
    public KirelCacheManager(IMemoryCache cache)
    {
        _cache = cache;
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="cacheKey"></param>
    /// <param name="getItemAsync"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public async Task<T> GetCache<T>(string cacheKey, Func<Task<T>> getItemAsync)
    {
        if (_cache.TryGetValue(cacheKey, out T item)) 
            return item;
        return await CreateCache(cacheKey , getItemAsync);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="cacheKey"></param>
    /// <param name="getItemAsync"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public async Task<T> CreateCache<T>(string cacheKey, Func<Task<T>> getItemAsync)
    {
        var item = await getItemAsync();
        if (item != null)
        {
            _cache.Set(cacheKey, item);
        }
        return item;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="cacheKey"></param>
    /// <param name="item"></param>
    /// <typeparam name="T"></typeparam>
    public async Task<T> UpdateCache<T>(string cacheKey, T item)
    {
        if (_cache.TryGetValue(cacheKey, out T cacheEntry))
        { 
            cacheEntry = item;
            return _cache.Set(cacheKey, cacheEntry);
        }
        // else
        // {
        //     return await CreateCache(cacheKey , getItemAsync);
        // }
        return item;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="cacheKey"></param>
    public void DeleteCache(string cacheKey)
    {
        _cache.Remove(cacheKey);
    }
}