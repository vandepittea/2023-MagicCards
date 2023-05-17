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

namespace Howest.MagicCards.GraphQL
{
    public class RootQuery : ObjectGraphType
    {
        public RootQuery(ICardRepository cardRepository, IArtistRepository artistRepository, IDistributedCache cache, IConfiguration config)
        {
            Name = "Query";

            FieldAsync<ListGraphType<CardType>>(
                name: "cards",
                description: "Get all cards",
                arguments: new QueryArguments(
                    new QueryArgument<StringGraphType> { Name = "power", Description = "Filter cards by power" },
                    new QueryArgument<StringGraphType> { Name = "toughness", Description = "Filter cards by toughness" },
                    new QueryArgument<IntGraphType> { Name = "pageNumber", Description = "Page number" },
                    new QueryArgument<IntGraphType> { Name = "pageSize", Description = "Page size" }
                ),
                resolve: async context =>
                {
                    string power = context.GetArgument<string>("power");
                    string toughness = context.GetArgument<string>("toughness");
                    int pageNumber = context.GetArgument<int?>("pageNumber") ?? 1;
                    int pageSize = context.GetArgument<int?>("pageSize") ?? int.Parse(config.GetSection("appSettings")["maxPageSize"]);

                    string cacheKey = $"cards_{power}_{toughness}_{pageNumber}_{pageSize}";
                    var cachedData = await cache.GetStringAsync(cacheKey);

                    if (cachedData != null)
                    {
                        List<Card> cachedCards = JsonSerializer.Deserialize<List<Card>>(cachedData);
                        return cachedCards;
                    }

                    IEnumerable<Card> cards = await cardRepository.GetCards();

                    if (!string.IsNullOrEmpty(power))
                        cards = cards.Where(c => c.Power == power);

                    if (!string.IsNullOrEmpty(toughness))
                        cards = cards.Where(c => c.Toughness == toughness);

                    List<Card> pagedCards = cards.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

                    var cacheOptions = new DistributedCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
                    };
                    var serializedData = JsonSerializer.Serialize(pagedCards);
                    await cache.SetStringAsync(cacheKey, serializedData, cacheOptions);

                    return pagedCards;
                }
            );

            FieldAsync<ListGraphType<ArtistType>>(
                name: "artists",
                description: "Get all artists",
                arguments: new QueryArguments(
                    new QueryArgument<IntGraphType> { Name = "limit", Description = "Number of artists to retrieve" }
                ),
                resolve: async context =>
                {
                    int? limit = context.GetArgument<int?>("limit");

                    IEnumerable<Artist> artists = await artistRepository.GetArtists();

                    if (limit.HasValue)
                        artists = artists.Take(limit.Value);

                    return artists.ToList();
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
