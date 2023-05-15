using AutoMapper;
using Howest.MagicCards.DAL.Models;
using Howest.MagicCards.DAL.Repositories;
using Howest.MagicCards.Shared.DTO;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Howest.MagicCards.MinimalAPI.Mappings
{
    public static class CardsEndpoints
    {
        public static void MapCardsEndpoints(this WebApplication app, string urlPrefix, IMapper mapper)
        {
            app.MapGet($"{urlPrefix}/deck", (IDeckRepository deckRepo) =>
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
            .WithTags("Deck actions")
            .Produces<List<CardInDeck>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest);

            app.MapPost($"{urlPrefix}/deck/{{card}}", (IDeckRepository deckRepo, CardInDeckDto cardInDeckDto) =>
            {
                try
                {
                    CardInDeck cardInDeck = mapper.Map<CardInDeck>(cardInDeckDto);

                    deckRepo.AddCardToDeck(cardInDeck);

                    return Results.Ok($"Card with id {cardInDeck._id} added to deck");
                }
                catch (Exception ex)
                {
                    return Results.BadRequest($"Error adding card to deck: {ex.Message}");
                }
            })
            .WithTags("Deck actions")
            .Accepts<CardInDeckDto>("application/json")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest);

            app.MapPut($"{urlPrefix}/deck/{{id}}", (IDeckRepository deckRepo, HttpRequest request) =>
            {
                try
                {
                    CardInDeckDto? cardInDeckDto = JsonSerializer.Deserialize<CardInDeckDto>(request.Body);

                    if (cardInDeckDto == null)
                    {
                        return Results.BadRequest("Invalid card provided");
                    }

                    CardInDeck cardInDeck = mapper.Map<CardInDeck>(cardInDeckDto);

                    deckRepo.IncrementCardCount(cardInDeck);
                    return Results.Ok($"Card with id {cardInDeck._id} is incremented");
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
            .WithTags("Deck actions")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound);

            app.MapDelete($"{urlPrefix}/deck/{{id}}", (IDeckRepository deckRepo, HttpRequest request) =>
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
            .WithTags("Deck actions")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound);

            app.MapDelete("$\"{urlPrefix}/deck/clear", (IDeckRepository deckRepo) =>
            {
                try
                {
                    deckRepo.ClearDeck();
                    return Results.Ok("Deck cleared successfully");
                }
                catch (Exception ex)
                {
                    return Results.BadRequest(ex.Message);
                }
            })
            .WithTags("Deck actions")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest);
        }
    }
}
