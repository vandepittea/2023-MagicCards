using Howest.MagicCards.DAL.Models;
using Howest.MagicCards.DAL.Repositories;
using HotChocolate.Types;
using GraphQL.Types;

namespace Howest.MagicCards.GraphQL.Types
{
    public class CardType : ObjectGraphType<Card>
    {
        public CardType(ICardRepository cardRepository)
        {
            Name = "Card";

            Field(c => c.Id, type: typeof(IdGraphType));

            Field(c => c.Name, type: typeof(StringGraphType));

            Field(c => c.ManaCost, type: typeof(StringGraphType));

            Field(c => c.Type, type: typeof(StringGraphType));

            Field(c => c.SetCode, type: typeof(StringGraphType));

            Field(c => c.RarityCode, type: typeof(StringGraphType));

            Field(c => c.Image, type: typeof(StringGraphType));

            Field(c => c.CardColors, type: typeof(ListGraphType<StringGraphType>));

            Field(c => c.CardTypes, type: typeof(ListGraphType<StringGraphType>));

            Field(c => c.Text, type: typeof(StringGraphType), nullable: true);

            Field(c => c.Flavor, type: typeof(StringGraphType), nullable: true);

            Field(c => c.Number, type: typeof(StringGraphType), nullable: true);

            Field(c => c.Power, type: typeof(StringGraphType));

            Field(c => c.Toughness, type: typeof(StringGraphType));

            Field(c => c.Layout, type: typeof(StringGraphType), nullable: true);

            Field(c => c.MultiverseId, type: typeof(IntGraphType), nullable: true);

            Field(c => c.OriginalImageUrl, type: typeof(StringGraphType), nullable: true);

            Field(c => c.OriginalText, type: typeof(StringGraphType), nullable: true);

            Field(c => c.OriginalType, type: typeof(StringGraphType), nullable: true);

            Field(c => c.MtgId, type: typeof(StringGraphType), nullable: true);

            Field(c => c.Variations, type: typeof(StringGraphType), nullable: true);

            Field<ArtistType>(
                                "Artist",
                                resolve: context => cardRepository.GetArtistById(context.Source.ArtistId ?? default)
            );
        }
    }
}
