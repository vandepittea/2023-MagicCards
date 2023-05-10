using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Howest.MagicCards.DAL.Repositories;
using Howest.MagicCards.DAL.Models;
using Howest.MagicCards.Shared;
using Howest.MagicCards.Shared.DTOs;

namespace Howest.MagicCards.WebAPI.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class CardsController : ControllerBase
    {
        private readonly CardRepository _cardRepository;

        public CardsController(ICardRepository cardRepository)
        {
            _cardRepository = new CardRepository();
        }

        [HttpGet]
        [ProducesResponseType(typeof(PagedResponse<IEnumerable<CardDto>>), 200)]
        public async Task<IActionResult> GetCards(
            [FromQuery] CardFilterDto filterDto)
        {
            var filter = new CardParameterFilter(filterDto);
            filter.Validate();

            var cards = await _cardRepository.GetFilteredAndSortedCards(filter);

            var pagedCards = cards.Skip((filterDto.PageNumber - 1) * filterDto.PageSize).Take(filterDto.PageSize).ToList();
            var totalCards = await _cardRepository.GetTotalCardCount(filter);
            var totalPages = (int)Math.Ceiling((double)totalCards / filterDto.PageSize);
            var pagedResponse = new PagedResponse<CardDto>(pagedCards, filterDto.PageNumber, filterDto.PageSize, totalPages, totalCards);

            return Ok(pagedResponse);
        }
    }
}
