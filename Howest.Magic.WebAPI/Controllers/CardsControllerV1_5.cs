using Howest.MagicCards.Shared.Wrappers;
using Howest.MagicCards.WebAPI.Extensions;
using Howest.MagicCards.WebAPI.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Howest.MagicCards.WebAPI.Controllers.V1_5
{
    [ApiVersion("1.5")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class CardsController : ControllerBase
    {
        private readonly IMemoryCache _cache;
        private readonly ICardRepository _cardRepository;
        private readonly IMapper _mapper;

        public CardsController(ICardRepository cardRepository, IMapper mapper, IMemoryCache memoryCache)
        {
            _cardRepository = cardRepository;
            _mapper = mapper;
            _cache = memoryCache;
        }

        [HttpGet]
        [ProducesResponseType(typeof(CardDto), 200)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 500)]
        public async Task<ActionResult<PagedResponse<IQueryable<CardDto>>>> GetCards([FromQuery] CardWebFilterV1_5 filter, [FromServices] IConfiguration config)
        {
            try
            {
                string cacheKey = GetCacheKey(filter);
                PagedResponse<IEnumerable<CardDto>> cachedResponse = CacheHelper.GetCachedResponse<PagedResponse<IEnumerable<CardDto>>>(_cache, cacheKey);
                if (cachedResponse != null)
                {
                    return Ok(cachedResponse);
                }

                IQueryable<Card> allCards = await _cardRepository.GetCards();

                if (allCards == null)
                {
                    return NotFound(ResponseHelper.NotFound<CardDto>("No cards found"));
                }

                IQueryable<Card> filteredCards = FilterCards(allCards, filter);

                IEnumerable<CardDto> pagedCards = GetPagedCards(filteredCards, filter, config);

                int totalCount = filteredCards.Count();
                int totalPages = (int)Math.Ceiling(totalCount / (double)filter.PageSize);

                PagedResponse<IEnumerable<CardDto>> response = new PagedResponse<IEnumerable<CardDto>>(pagedCards, filter.PageNumber, filter.PageSize, totalCount, totalPages);

                CacheHelper.SetCache(_cache, cacheKey, response);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ResponseHelper.InternalServerError<CardDto>(ex.Message));
            }
        }

        [HttpGet("{id:int}", Name = "GetCardById")]
        [ProducesResponseType(typeof(CardDetailDto), 200)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 500)]
        public async Task<ActionResult<CardDetailDto>> GetCard(int id)
        {
            try
            {
                string cacheKey = $"card_{id}";
                CardDetailDto cachedCard = CacheHelper.GetCachedResponse<CardDetailDto>(_cache, cacheKey);
                if (cachedCard != null)
                {
                    return cachedCard;
                }

                Card card = await _cardRepository.GetCardById(id);

                if (card == null)
                {
                    return NotFound(ResponseHelper.NotFound<CardDetailDto>("Card not found"));
                }

                CardDetailDto cardDetailDto = _mapper.Map<CardDetailDto>(card);

                CacheHelper.SetCache(_cache, cacheKey, cardDetailDto);

                return Ok(cardDetailDto);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ResponseHelper.InternalServerError<CardDetailDto>(ex.Message));
            }
        }

        private string GetCacheKey(CardWebFilterV1_5 filter)
        {
            return $"cards_{JsonSerializer.Serialize(filter)}_{filter.SortBy}";
        }

        private IQueryable<Card> FilterCards(IQueryable<Card> cards, CardWebFilterV1_5 filter)
        {
            return cards
                .FilterBySet(filter.SetName)
                .FilterByArtist(filter.ArtistName)
                .FilterByRarity(filter.RarityName)
                .FilterByCardType(filter.TypeName)
                .FilterByCardName(filter.CardName)
                .FilterByCardText(filter.CardText)
                .Sort(filter.SortBy);
        }

        private IEnumerable<CardDto> GetPagedCards(IQueryable<Card> cards, CardWebFilterV1_5 filter, IConfiguration config)
        {
            return cards
                .ToPagedList<Card>(filter.PageNumber, filter.PageSize, int.Parse(config.GetSection("appSettings")["minPageSize"]), int.Parse(config.GetSection("appSettings")["maxPageSize"]))
                .ProjectTo<CardDto>(_mapper.ConfigurationProvider)
                .ToList();
        }
    }
}