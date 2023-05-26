using Howest.MagicCards.Shared.Wrappers;
using Microsoft.Extensions.Caching.Memory;
using System;

namespace Howest.MagicCards.WebAPI.Helpers
{
    public static class CacheHelper
    {
        public static T GetCachedResponse<T>(IMemoryCache cache, string cacheKey) where T : class
        {
            return cache.Get<T>(cacheKey);
        }

        public static void SetCache<T>(IMemoryCache cache, string cacheKey, T data) where T : class
        {
            MemoryCacheEntryOptions cacheOptions = new MemoryCacheEntryOptions()
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
            };

            cache.Set(cacheKey, data, cacheOptions);
        }
    }
}
