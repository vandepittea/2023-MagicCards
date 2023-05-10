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
        public async Task<IActionResult> GetCards(
            [FromQuery] string setName,
            [FromQuery] string artistName,
            [FromQuery] string rarity,
            [FromQuery] string type,
            [FromQuery] string searchQuery,
            [FromQuery] string sortBy,
            [FromQuery] bool sortAscending = true,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 50)
        {
            var cards = await _cardRepository.GetFilteredAndSortedCards(setName, artistName, rarity, type, searchQuery, sortBy, sortAscending, page, pageSize);
            var totalCards = cards.Count();
            var pagedCards = cards.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            var pagedResponse = new PagedResponse<IEnumerable<Card>>(pagedCards, page, pageSize, totalCards);
            return Ok(pagedResponse);
        }
    }
}
