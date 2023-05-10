using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Howest.MagicCards.DAL.Repositories;
using Howest.MagicCards.DAL.Models;

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
        [ProducesResponseType(typeof(PagedResponse<CardDto>), 200)]
        public async Task<IActionResult> GetCards(
            [FromQuery] string setName,
            [FromQuery] string artistName,
            [FromQuery] string rarity,
            [FromQuery] string type,
            [FromQuery] string searchQuery,
            [FromQuery] string sortBy,
            [FromQuery] bool sortAscending = true,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 50)
        {
            var paginationFilter = new PaginationFilter { PageNumber = pageNumber, PageSize = pageSize };
            var cards = await _cardRepository.GetFilteredAndSortedCards(setName, artistName, rarity, type, searchQuery, sortBy, sortAscending, paginationFilter.PageNumber, paginationFilter.PageSize);
            var totalCards = cards.Count();
            var pagedCards = cards.Skip((paginationFilter.PageNumber - 1) * paginationFilter.PageSize).Take(paginationFilter.PageSize).ToList();
            var cardDtos = pagedCards.Select(card => new CardDto(card));
            var pagedResponse = new PagedResponse<CardDto>(cardDtos, paginationFilter.PageNumber, paginationFilter.PageSize, totalCards);
            return Ok(pagedResponse);
        }
    }
}
