using Howest.MagicCards.DAL.Repositories;
using Howest.MagicCards.GraphQL.Types;
using System.Collections.Generic;
using System.Linq;
using GraphQL.Types;
using GraphQL;
using Howest.MagicCards.DAL.Models;
using CardType = Howest.MagicCards.GraphQL.Types.CardType;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using Microsoft.Extensions.Caching.Memory;
using Howest.MagicCards.Shared.Filters;
using Howest.MagicCards.Shared.Extensions;
using Howest.MagicCards.WebAPI.Extensions;

namespace Howest.MagicCards.GraphQL
{
    public class RootQuery : ObjectGraphType
    {
        public RootQuery(ICardRepository cardRepository, IArtistRepository artistRepository, IMemoryCache cache, IConfiguration config)
        {
            Name = "Query";

            FieldAsync<ListGraphType<CardType>>(
                name: "cards",
                description: "Get all cards",
                arguments: new QueryArguments(
                    new QueryArgument<StringGraphType> { Name = "power", Description = "Filter cards by power" },
                    new QueryArgument<StringGraphType> { Name = "toughness", Description = "Filter cards by toughness" },
                    new QueryArgument<IntGraphType> { Name = "pageNumber", Description = "Page number" },
                    new QueryArgument<IntGraphType> { Name = "pageSize", Description = "Page size" },
                    new QueryArgument<StringGraphType> { Name = "sort", Description = "Sort cards by a specific field" }
                ),
                resolve: async context =>
                {
                    string power = context.GetArgument<string>("power");
                    string toughness = context.GetArgument<string>("toughness");
                    int pageNumber = context.GetArgument<int?>("pageNumber") ?? 1;
                    int pageSize = context.GetArgument<int?>("pageSize") ?? int.Parse(config.GetSection("appSettings")["maxPageSize"]);
                    string sort = context.GetArgument<string>("sort");

                    CardGraphFilter filter = new CardGraphFilter
                    {
                        Power = power,
                        Toughness = toughness,
                        PageNumber = pageNumber,
                        PageSize = pageSize,
                        SortBy = sort
                    };

                    string cacheKey = $"cards_{JsonSerializer.Serialize(filter)}";
                    if (cache.TryGetValue(cacheKey, out List<Card> cachedCards))
                    {
                        return cachedCards;
                    }

                    IQueryable<Card> query = await cardRepository.GetCards();

                    query = query.FilterByPower(power)
                                 .FilterByToughness(toughness)
                                 .Sort(filter.SortBy);

                    List<Card> pagedCards = query.ToPagedList(filter.PageNumber, filter.PageSize, int.Parse(config.GetSection("appSettings")["minPageSize"]), int.Parse(config.GetSection("appSettings")["maxPageSize"]))
                                                .ToList();

                    MemoryCacheEntryOptions cacheOptions = new MemoryCacheEntryOptions()
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
                    };

                    cache.Set(cacheKey, pagedCards, cacheOptions);

                    return pagedCards;
                }
            );


            FieldAsync<ListGraphType<ArtistType>>(
                name: "artists",
                description: "Get all artists",
                arguments: new QueryArguments(
                    new QueryArgument<IntGraphType> { Name = "limit", Description = "Number of artists to retrieve" },
                         new QueryArgument<IntGraphType> { Name = "pageNumber", Description = "Page number" },
                         new QueryArgument<IntGraphType> { Name = "pageSize", Description = "Page size" },
                         new QueryArgument<StringGraphType> { Name = "sort", Description = "Sort artists by a specific field" }
                ),
                resolve: async context =>
                {
                    int? limit = context.GetArgument<int?>("limit");
                    string sort = context.GetArgument<string>("sort");

                    ArtistFilter filter = new ArtistFilter
                    {
                        SortBy = sort,
                        Limit = limit.HasValue ? (int)limit : int.MaxValue
                    };

                    string cacheKey = $"artists_{JsonSerializer.Serialize(filter)}";
                    if (cache.TryGetValue(cacheKey, out List<Artist> cachedArtists))
                    {
                        return cachedArtists;
                    }

                    IQueryable<Artist> query = await artistRepository.GetArtists();

                    query = query.Sort(filter.SortBy)
                                 .Limit(filter.Limit);

                    List<Artist> artists = query.ToList();

                    MemoryCacheEntryOptions cacheOptions = new MemoryCacheEntryOptions()
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
                    };

                    cache.Set(cacheKey, artists, cacheOptions);

                    return artists;
                }
            );


            Field<ArtistType>(
                name: "artist",
                description: "Get a specific artist by ID",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<IdGraphType>> { Name = "id", Description = "ID of the artist" }
                ),
                resolve: context =>
                {
                    long id = context.GetArgument<long>("id");
                    return artistRepository.GetArtistById(id);
                }
            );
        }
    }
}
