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

            app.MapPost($"{urlPrefix}/deck/{{id}}", (DeckRepository deckRepo, HttpRequest request) =>
            {
                try
                {
                    if (!int.TryParse(request.RouteValues["id"] as string, out int id))
                    {
                        return Results.BadRequest("Invalid card id provided");
                    }

                    deckRepo.AddCardToDeck(id);
                    return Results.Ok($"Card with id {id} added to deck");
                }
                catch (Exception ex)
                {
                    return Results.BadRequest($"Error adding card to deck: {ex.Message}");
                }
            })
            .WithTags("Add a card to the deck")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest);

            app.MapPut($"{urlPrefix}/deck/{{id}}", (DeckRepository deckRepo, HttpRequest request) =>
            {
                try
                {
                    if (!int.TryParse(request.RouteValues["id"] as string, out int id))
                    {
                        return Results.BadRequest("Invalid card id provided");
                    }

                    deckRepo.IncrementCardCount(id);
                    return Results.Ok($"Card with id {id} is incremented");
                }
                catch (ArgumentException ex)
                {
                    return Results.NotFound($"Error updating card count: {ex.Message}");
                }
                catch (Exception ex)
                {
                    return Results.BadRequest($"Error updating card count: {ex.Message}");
                }
            })
            .WithTags("Update the count of a card in the deck")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound);

            app.MapDelete($"{urlPrefix}/deck/{{id}}", (DeckRepository deckRepo, HttpRequest request) =>
            {
                try
                {
                    if (!int.TryParse(request.RouteValues["id"] as string, out int id))
                    {
                        return Results.BadRequest("Invalid card id provided");
                    }

                    deckRepo.RemoveCardFromDeck(id);
                    return Results.Ok($"Card with id {id} is deleted");
                }
                catch (ArgumentException ex)
                {
                    return Results.NotFound($"Error removing card: {ex.Message}");
                }
                catch (Exception ex)
                {
                    return Results.BadRequest($"Error removing card: {ex.Message}");
                }
            })
            .WithTags("Update the count of a card in the deck")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound);

            app.MapDelete("$\"{urlPrefix}/deck/clear", (IDeckRepository repository) =>
            {
                try
                {
                    repository.ClearDeck();
                    return Results.Ok("Deck cleared successfully");
                }
                catch (Exception ex)
                {
                    return Results.BadRequest(ex.Message);
                }
            })
            .WithTags("Clear the deck")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest);
        }
    }
}
