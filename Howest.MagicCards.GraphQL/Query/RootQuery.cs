using CardType = Howest.MagicCards.GraphQL.Types.CardType;

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

                    return await CardQueryHelper.GetCards(cardRepository, cache, config, power, toughness, pageNumber, pageSize, sort);
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

                    return await ArtistQueryHelper.GetArtists(artistRepository, cache, config, limit, sort);
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