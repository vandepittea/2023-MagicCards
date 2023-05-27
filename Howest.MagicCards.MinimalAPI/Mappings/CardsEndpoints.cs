namespace Howest.MagicCards.MinimalAPI.Mappings
{
    public static class CardsEndpoints
    {
        public static void MapCardsEndpoints(this WebApplication app, string urlPrefix, IMapper mapper, IConfiguration configuration)
        {
            app.MapGet($"{urlPrefix}/deck", (IDeckRepository deckRepo) => GetDeck(deckRepo, mapper))
                .WithTags("Deck actions")
                .Produces<List<CardInDeck>>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status400BadRequest);

            app.MapPost($"{urlPrefix}/deck", (IDeckRepository deckRepo, CardInDeckDto cardInDeckDto, IValidator<CardInDeckDto> validator) =>
                AddCardToDeck(deckRepo, cardInDeckDto, validator, mapper, configuration))
                .WithTags("Deck actions")
                .Accepts<CardInDeckDto>("application/json")
                .Produces(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status400BadRequest);

            app.MapPut($"{urlPrefix}/deck/", (IDeckRepository deckRepo, CardInDeckDto cardInDeckDto, IValidator<CardInDeckDto> validator) =>
                UpdateCardCount(deckRepo, cardInDeckDto, validator, mapper))
                .WithTags("Deck actions")
                .Accepts<CardInDeckDto>("application/json")
                .Produces(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status400BadRequest)
                .Produces(StatusCodes.Status404NotFound);

            app.MapDelete($"{urlPrefix}/deck/{{id}}", (IDeckRepository deckRepo, int id, IValidator<int> validator) =>
                RemoveCardFromDeck(deckRepo, id, validator))
                .WithTags("Deck actions")
                .Produces(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status400BadRequest)
                .Produces(StatusCodes.Status404NotFound);

            app.MapDelete($"{urlPrefix}/deck/clear", (IDeckRepository deckRepo) =>
                ClearDeck(deckRepo))
                .WithTags("Deck actions")
                .Produces(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status400BadRequest);
        }

        private static IResult GetDeck(IDeckRepository deckRepo, IMapper mapper)
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
        }

        private static IResult AddCardToDeck(IDeckRepository deckRepo, CardInDeckDto cardInDeckDto, IValidator<CardInDeckDto> validator, IMapper mapper, IConfiguration configuration)
        {
            try
            {
                ValidationResult validationResult = validator.Validate(cardInDeckDto);

                if (validationResult.IsValid)
                {
                    CardInDeck cardInDeck = mapper.Map<CardInDeck>(cardInDeckDto);

                    deckRepo.AddCardToDeck(cardInDeck, configuration.GetValue<int>("AppSettings:MaxCardsDeck"));

                    return Results.Ok($"Card with id {cardInDeck.Id} with count {cardInDeck.Count} added to deck");
                }
                else
                {
                    return Results.BadRequest(BuildModelState(validationResult.Errors));
                }
            }
            catch (Exception ex)
            {
                return Results.BadRequest($"Error adding card to deck: {ex.Message}");
            }
        }

        private static IResult UpdateCardCount(IDeckRepository deckRepo, CardInDeckDto cardInDeckDto, IValidator<CardInDeckDto> validator, IMapper mapper)
        {
            try
            {
                ValidationResult validationResult = validator.Validate(cardInDeckDto);

                if (validationResult.IsValid)
                {
                    CardInDeck cardInDeck = mapper.Map<CardInDeck>(cardInDeckDto);

                    deckRepo.UpdateCardCount(cardInDeck);
                    return Results.Ok($"Card with id {cardInDeck.Id} has a count of {cardInDeck.Count}");
                }
                else
                {
                    return Results.BadRequest(BuildModelState(validationResult.Errors));
                }
            }
            catch (ArgumentException ex)
            {
                return Results.NotFound($"Error updating card count: {ex.Message}");
            }
            catch (Exception ex)
            {
                return Results.BadRequest($"Error updating card count: {ex.Message}");
            }
        }

        private static IResult RemoveCardFromDeck(IDeckRepository deckRepo, int id, IValidator<int> validator)
        {
            try
            {
                ValidationResult validationResult = validator.Validate(id);

                if (validationResult.IsValid)
                {
                    deckRepo.RemoveCardFromDeck(id);
                    return Results.Ok($"Card with id {id} is deleted");
                }
                else
                {
                    return Results.BadRequest(BuildModelState(validationResult.Errors));
                }
            }
            catch (ArgumentException ex)
            {
                return Results.NotFound($"Error removing card: {ex.Message}");
            }
            catch (Exception ex)
            {
                return Results.BadRequest($"Error removing card: {ex.Message}");
            }
        }

        private static IResult ClearDeck(IDeckRepository deckRepo)
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
        }

        private static ModelStateDictionary BuildModelState(IEnumerable<ValidationFailure> validationErrors)
        {
            ModelStateDictionary modelState = new ModelStateDictionary();
            foreach (ValidationFailure error in validationErrors)
            {
                modelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }
            return modelState;
        }
    }
}