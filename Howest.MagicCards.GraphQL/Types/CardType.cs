﻿namespace Howest.MagicCards.GraphQL.Types
{
    public class CardType : ObjectGraphType<Card>
    {
        public CardType(IArtistRepository artistRepository)
        {
            Name = "Card";

            Field(c => c.Id, type: typeof(IdGraphType));

            Field(c => c.Name, type: typeof(StringGraphType));

            Field(c => c.ConvertedManaCost, type: typeof(StringGraphType));

            Field(c => c.Type, type: typeof(StringGraphType));

            Field(c => c.SetCode, type: typeof(StringGraphType));

            Field(c => c.RarityCode, type: typeof(StringGraphType));

            Field(c => c.Image, type: typeof(StringGraphType));

            Field(c => c.Text, type: typeof(StringGraphType), nullable: true);

            Field(c => c.Flavor, type: typeof(StringGraphType), nullable: true);

            Field(c => c.Number, type: typeof(StringGraphType), nullable: true);

            Field(c => c.Power, type: typeof(StringGraphType));

            Field(c => c.Toughness, type: typeof(StringGraphType));

            Field(c => c.Layout, type: typeof(StringGraphType), nullable: true);

            FieldAsync<ArtistType>(
                                "Artist",
                                resolve: async context =>
                                {
                                    using (MtgDbContext dbContext = new MtgDbContext())
                                    {
                                        return await artistRepository.GetArtistById(context.Source.ArtistId ?? default, dbContext);
                                    }
                                }
            );
        }
    }
}
