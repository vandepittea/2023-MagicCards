namespace Howest.MagicCards.GraphQL.Helpers
{
    public static class ArtistQueryHelper
    {
        public static async Task<List<Artist>> GetArtists(IArtistRepository artistRepository, IMemoryCache cache, IConfiguration config, int? limit, string sort)
        {
            ArtistFilter filter = new ArtistFilter
            {
                SortBy = sort,
                Limit = limit.HasValue ? (int)limit : int.MaxValue
            };

            string cacheKey = GetCacheKey(filter);
            if (CacheHelper.GetCachedResponse<List<Artist>>(cache, cacheKey) is List<Artist> cachedArtists)
            {
                return cachedArtists;
            }

            IQueryable<Artist> query = await artistRepository.GetArtists();

            query = query.Sort(filter.SortBy)
                         .Limit(filter.Limit);

            List<Artist> artists = query.ToList();

            CacheHelper.SetCache(cache, cacheKey, artists);

            return artists;
        }

        private static string GetCacheKey(ArtistFilter filter)
        {
            return $"artists_{JsonSerializer.Serialize(filter)}";
        }
    }
}