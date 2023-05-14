using AutoMapper;
using Howest.MagicCards.DAL.Models;
using Howest.MagicCards.DAL.Repositories;
using Howest.MagicCards.Shared.DTO;
using System.Text.Json;

namespace Howest.MagicCards.MinimalAPI.Mappings
{
    public static class CardsEndpoints
    {
        public static void MapCardsEndpoints(this WebApplication app, string urlPrefix, IMapper mapper)
        {
            app.MapGet($"{urlPrefix}/deck", (DeckRepository deckRepo) =>
            {
                try
                {
                    IEnumerable<CardInDeck> cardsInDeck = deckRepo.GetCards();
                    List<CardInDeckDto> cardInDeckDtos = mapper.Map<IEnumerable<CardInDeck>, List<CardInDeckDto>>(cardsInDeck);
                    return Results.Ok(cardInDeckDtos);
                }
                catch (Exception ex)
                {
                    return Results.BadRequest($"Error getting deck: {ex.Message}");
                }
            })
            .WithTags("Get the current deck")
            .Produces<List<CardInDeck>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest);
        }
    }
}
