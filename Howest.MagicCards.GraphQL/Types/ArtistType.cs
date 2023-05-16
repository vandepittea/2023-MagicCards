using Howest.MagicCards.DAL.Models;
using HotChocolate.Types;
using Howest.MagicCards.DAL.Repositories;
using System.Collections.Generic;
using GraphQL.Types;
using Amazon.Runtime.Internal.Util;

namespace Howest.MagicCards.GraphQL.Types
{
    public class ArtistType : ObjectGraphType<Artist>
    {
        public ArtistType(ICardRepository cardRepository)
        {
            Name = "Artist";

            Field(a => a.Id, type: typeof(IdGraphType));
            Field(a => a.FullName, type: typeof(StringGraphType));

            Field<CardType>(
                                "Cards",
                                resolve: context => cardRepository.GetCardsByArtistId(context.Source.Id)
            );
        }
    }
}
