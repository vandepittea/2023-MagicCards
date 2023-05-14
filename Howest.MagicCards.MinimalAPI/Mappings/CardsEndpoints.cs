using AutoMapper;
using Howest.MagicCards.DAL.Models;
using Howest.MagicCards.DAL.Repositories;
using Howest.MagicCards.Shared.DTO.Howest.MagicCards.Shared.DTO;
using Howest.MagicCards.Shared.Mappings;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using System.Text.Json;

namespace Howest.MagicCards.MinimalAPI.Mappings
{
    public static class CardsEndpoints
    {
        public static void MapCardsEndpoints(this WebApplication app, string urlPrefix, IMapper mapper)
        {
            app.MapPost($"{urlPrefix}/cards", (MongoDbCardRepository cardRepo, HttpRequest request) =>
            {
                try
                {
                    using (StreamReader streamReader = new StreamReader(request.Body))
                    {
                        string json = streamReader.ReadToEnd();
                        CardWriteDto? newCardDto = JsonSerializer.Deserialize<CardWriteDto>(json) ?? throw new ArgumentException("Invalid card update data provided");

                        Card newCard = mapper.Map<CardWriteDto, Card>(newCardDto);
                        cardRepo.AddCard(newCard);

                        return Results.Created($"{urlPrefix}/cards/{newCard.Id}", newCard);
                    }
                }
                catch (ArgumentException ex)
                {
                    return Results.BadRequest($"Error updating card: {ex.Message}");
                }
                catch (Exception ex)
                {
                    return Results.BadRequest($"Error adding card: {ex.Message}");
                }
            })
            .WithTags("Add a new card")
            .Produces(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest);

            app.MapPut($"{urlPrefix}/cards/{{id}}", (SqlCardRepository sqlCardRepo, MongoDbCardRepository mongoCardRepo, HttpRequest request) =>
            {
                try
                {
                    using (StreamReader streamReader = new StreamReader(request.Body))
                    {
                        string json = streamReader.ReadToEnd();
                        CardUpdateDto updatedCardDto = JsonSerializer.Deserialize<CardUpdateDto>(json) ?? throw new ArgumentException("Invalid card update data provided");

                        Card? foundCard = sqlCardRepo.GetCardById(updatedCardDto.Id).Result;

                        if (foundCard == null)
                        {
                            return Results.NotFound($"No card with id {updatedCardDto.Id} found");
                        }

                        Card updatedCard = mapper.Map<CardUpdateDto, Card>(updatedCardDto);

                        mongoCardRepo.UpdateCard(updatedCard);
                        return Results.Ok(updatedCard);
                    }
                }
                catch (ArgumentException ex)
                {
                    return Results.BadRequest($"Error updating card: {ex.Message}");
                }
                catch (Exception ex)
                {
                    return Results.BadRequest($"Error updating card: {ex.Message}");
                }
            })
            .WithTags("Update an existing card")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest);

            app.MapDelete($"{urlPrefix}/cards/{{id}}", (SqlCardRepository sqlCardRepo, MongoDbCardRepository mongoCardRepo, HttpRequest request) =>
            {
                try
                {
                    if (!long.TryParse(request.RouteValues["id"] as string, out long id))
                    {
                        return Results.BadRequest("Invalid card id provided");
                    }

                    Card? foundCard = sqlCardRepo.GetCardById(id).Result;
                    if (foundCard == null)
                    {
                        return Results.NotFound($"No card with id {id} found");
                    }

                    mongoCardRepo.DeleteCard(id);
                    return Results.Ok($"Card with id {id} is deleted!");
                }
                catch (Exception ex)
                {
                    return Results.BadRequest($"Error deleting card: {ex.Message}");
                }
            })
            .WithTags("Delete an existing card")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);
        }
    }
}
