using Howest.MagicCards.DAL.Repositories;
using Microsoft.Extensions.Caching.Memory;
using System.Text.Json;
using Howest.MagicCards.DAL.Models;
using Howest.MagicCards.Shared.Filters;
using Howest.MagicCards.Shared.Extensions;
using Howest.MagicCards.WebAPI.Helpers;
using Howest.MagicCards.WebAPI.Extensions;

namespace Howest.MagicCards.GraphQL.Helpers
{
    public static class CardQueryHelper
    {
        public static async Task<List<Card>> GetCards(ICardRepository cardRepository, IMemoryCache cache, IConfiguration config, string power, string toughness, int pageNumber, int pageSize, string sort)
        {
            CardGraphFilter filter = CreateFilter(power, toughness, pageNumber, pageSize, sort);

            string cacheKey = GetCacheKey(filter);
            if (CacheHelper.GetCachedResponse<List<Card>>(cache, cacheKey) is List<Card> cachedCards)
            {
                return cachedCards;
            }

            IQueryable<Card> query = await cardRepository.GetCards();

            query = ApplyFilters(query, filter);

            List<Card> pagedCards = GetPagedCards(query, filter, config);

            CacheHelper.SetCache(cache, cacheKey, pagedCards);

            return pagedCards;
        }

        private static CardGraphFilter CreateFilter(string power, string toughness, int pageNumber, int pageSize, string sort)
        {
            return new CardGraphFilter
            {
                Power = power,
                Toughness = toughness,
                PageNumber = pageNumber,
                PageSize = pageSize,
                SortBy = sort
            };
        }

        private static string GetCacheKey(CardGraphFilter filter)
        {
            return $"cards_{JsonSerializer.Serialize(filter)}";
        }

        private static IQueryable<Card> ApplyFilters(IQueryable<Card> query, CardGraphFilter filter)
        {
            query = query.FilterByPower(filter.Power)
                         .FilterByToughness(filter.Toughness)
                         .Sort(filter.SortBy); 

            return query;
        }

        private static List<Card> GetPagedCards(IQueryable<Card> query, CardGraphFilter filter, IConfiguration config)
        {
            List<Card> pagedCards = query.ToPagedList(filter.PageNumber, filter.PageSize, int.Parse(config.GetSection("appSettings")["minPageSize"]), int.Parse(config.GetSection("appSettings")["maxPageSize"]))
                                        .ToList();

            return pagedCards;
        }
    }
}