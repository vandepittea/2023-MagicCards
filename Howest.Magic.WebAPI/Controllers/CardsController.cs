using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Howest.MagicCards.DAL.Repositories;
using Howest.MagicCards.DAL.Models;
using Howest.MagicCards.Shared;
using Howest.MagicCards.Shared.DTOs;
using FluentValidation;

namespace Howest.MagicCards.WebAPI.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class CardsController : ControllerBase
    {
        private readonly CardRepository _cardRepository;
        private readonly CardValidator _validator;

        public CardsController(CardRepository cardRepository, CardValidator validator)
        {
            _cardRepository = cardRepository;
            _validator = validator;
        }

        [HttpGet]
        [ProducesResponseType(typeof(PagedResponse<IEnumerable<CardDto>>), 200)]
        public async Task<IActionResult> GetCards(
            [FromQuery] CardDetailDto filterDto)
        {
            var validationResult = _validator.Validate(filterDto);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var filter = new CardParameterFilter(filterDto);
            var cards = await _cardRepository.GetCards(filter);

            var totalCards = await _cardRepository.GetTotalCardCount(filter);
            var totalPages = (int)Math.Ceiling((double)totalCards / filterDto.PageSize);
            var pagedResponse = new PagedResponse<CardDto>(cards, filterDto.PageNumber, filterDto.PageSize, totalPages, totalCards);

            return Ok(pagedResponse);
        }
    }
}
