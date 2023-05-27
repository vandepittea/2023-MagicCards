namespace Howest.MagicCards.GraphQL.Types
{
    public class ArtistType : ObjectGraphType<Artist>
    {
        public ArtistType(ICardRepository cardRepository)
        {
            Name = "Artist";

            Field(a => a.Id, type: typeof(IdGraphType));
            Field(a => a.FullName, type: typeof(StringGraphType));

            FieldAsync<ListGraphType<CardType>>(
                                "Cards",
                                resolve: async context => 
                                {
                                    using (MtgDbContext dbContext = new MtgDbContext())
                                    {
                                        return await cardRepository.GetCardsByArtistId(context.Source.Id, dbContext);
                                    }
                                }
            );
        }
    }
}
