using AutoMapper;
using Howest.MagicCards.DAL.Models;
using Howest.MagicCards.DAL.Repositories;
using Howest.MagicCards.Shared.DTO.Howest.MagicCards.Shared.DTO;
using Howest.MagicCards.Shared.Mappings;

namespace Howest.MagicCards.MinimalAPI.Mappings
{
    public static class CardsEndpoints
    {
        public static void MapCardsEndpoints(this WebApplication app, string urlPrefix, IMapper mapper)
        {
            app.MapPost($"{urlPrefix}/cards", (MongoDbCardRepository cardRepo, CardWriteDto newCardDto) =>
            {
                try
                {
                    Card newCard = mapper.Map<CardWriteDto, Card>(newCardDto);
                    cardRepo.AddCard(newCard);
                    return Results.Created($"{urlPrefix}/cards/{newCard.Id}", newCard);
                }
                catch (Exception ex)
                {
                    return Results.BadRequest($"Error adding card: {ex.Message}");
                }
            })
            .WithTags("Add a new card")
            .Accepts<CardWriteDto>("application/json")
            .Produces(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest);

            app.MapPut($"{urlPrefix}/cards/{{id}}", (SqlCardRepository sqlCardRepo, MongoDbCardRepository mongoCardRepo, CardUpdateDto updatedCardDto) =>
            {
                try
                {
                    Card foundCard = sqlCardRepo.GetCardById(updatedCardDto.Id).Result;
                    if (foundCard == null)
                    {
                        return Results.NotFound($"No card with id {updatedCardDto.Id} found");
                    }

                    Card updatedCard = mapper.Map<CardUpdateDto, Card>(updatedCardDto);

                    mongoCardRepo.UpdateCard(updatedCard);
                    return Results.Ok(updatedCard);
                }
                catch (Exception ex)
                {
                    return Results.BadRequest($"Error updating card: {ex.Message}");
                }
            })
            .WithTags("Update an existing card")
            .Accepts<CardUpdateDto>("application/json")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest);

            app.MapDelete($"{urlPrefix}/cards/{{id}}", (SqlCardRepository sqlCardRepo, MongoDbCardRepository mongoCardRepo, long id) =>
            {
                try
                {
                    Card foundCard = sqlCardRepo.GetCardById(id).Result;
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

        public static void AddCardsServices(this IServiceCollection services)
        {
            services.AddSingleton<MongoDbCardRepository>();
        }
    }
}
