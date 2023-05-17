using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using System.Text.Json;

namespace Howest.MagicCards.WebAPI.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class CardsController : ControllerBase
    {
        private readonly IDistributedCache _cache;
        private readonly ICardRepository _cardRepository;
        private readonly IMapper _mapper;

        public CardsController(ICardRepository cardRepository, IMapper mapper, IDistributedCache memoryCache)
        {
            _cardRepository = cardRepository;
            _mapper = mapper;
            _cache = memoryCache;
        }

        [HttpGet]
        [ProducesResponseType(typeof(CardDto), 200)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 500)]
        public async Task<ActionResult<PagedResponse<IQueryable<CardDto>>>> GetCards([FromQuery] CardFilter filter)
        {
            try
            {
                string cacheKey = $"cards_{JsonSerializer.Serialize(filter)}_{filter.SortBy}";
                string cachedData = await _cache.GetStringAsync(cacheKey);

                if (cachedData != null)
                {
                    PagedResponse<IQueryable<CardDto>> cachedResponse = JsonSerializer.Deserialize<PagedResponse<IQueryable<CardDto>>>(cachedData);
                    return Ok(cachedResponse);
                }

                IQueryable<Card> allCards = (IQueryable<Card>)await _cardRepository.GetCards();

                if (allCards == null)
                {
                    return NotFound(new Response<CardDto>()
                    {
                        Errors = new string[] { "404" },
                        Message = "No cards found"
                    });
                }

                IQueryable<Card> filteredCards = allCards
                    .FilterBySet(filter.SetCode)
                    .FilterByArtist(filter.ArtistName)
                    .FilterByRarity(filter.RarityCode)
                    .FilterByCardType(filter.CardType)
                    .FilterByCardName(filter.CardName)
                    .FilterByCardText(filter.CardText);

                IEnumerable<CardDto> pagedCards = filteredCards
                    .ToPagedList<Card>(filter.PageNumber, filter.PageSize)
                    .ProjectTo<CardDto>(_mapper.ConfigurationProvider)
                    .ToList();

                int totalCount = filteredCards.Count();
                int totalPages = (int)Math.Ceiling(totalCount / (double)filter.PageSize);

                PagedResponse<IEnumerable<CardDto>> response = new PagedResponse<IEnumerable<CardDto>>(pagedCards, filter.PageNumber, filter.PageSize, totalCount, totalPages);
                string responseData = JsonSerializer.Serialize(response);

                DistributedCacheEntryOptions cacheOptions = new DistributedCacheEntryOptions()
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
                };

                await _cache.SetStringAsync(cacheKey, responseData, cacheOptions);

                return Ok(response);
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new Response<CardDto>()
                    {
                        Succeeded = false,
                        Errors = new string[] { $"Status code: {StatusCodes.Status500InternalServerError}" },
                        Message = $"({ex.Message}) "
                    });
            }
        }
    }
}

namespace Howest.MagicCards.WebAPI.Controllers.V2
{
    [ApiVersion("2.0")]
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
        public async Task<ActionResult<PagedResponse<IQueryable<CardDto>>>> GetCards([FromQuery] CardFilter filter)
        {
            try
            {
                string cacheKey = $"cards_{JsonSerializer.Serialize(filter)}_{filter.SortBy}";
                if (_cache.TryGetValue(cacheKey, out PagedResponse<IEnumerable<CardDto>> cachedResponse))
                {
                    return Ok(cachedResponse);
                }

                IQueryable<Card> allCards = (IQueryable<Card>)await _cardRepository.GetCards();

                if (allCards == null)
                {
                    return NotFound(new Response<CardDto>()
                    {
                        Errors = new string[] { "404" },
                        Message = "No cards found"
                    });
                }

                IQueryable<Card> filteredCards = allCards
                    .FilterBySet(filter.SetCode)
                    .FilterByArtist(filter.ArtistName)
                    .FilterByRarity(filter.RarityCode)
                    .FilterByCardType(filter.CardType)
                    .FilterByCardName(filter.CardName)
                    .FilterByCardText(filter.CardText)
                    .Sort(filter.SortBy);

                IEnumerable<CardDto> pagedCards = filteredCards
                    .ToPagedList<Card>(filter.PageNumber, filter.PageSize)
                    .ProjectTo<CardDto>(_mapper.ConfigurationProvider)
                    .ToList();

                int totalCount = filteredCards.Count();
                int totalPages = (int)Math.Ceiling(totalCount / (double)filter.PageSize);

                PagedResponse<IEnumerable<CardDto>> response = new PagedResponse<IEnumerable<CardDto>>(pagedCards, filter.PageNumber, filter.PageSize, totalCount, totalPages);

                MemoryCacheEntryOptions cacheOptions = new MemoryCacheEntryOptions()
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
                };

                _cache.Set(cacheKey, response, cacheOptions);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new Response<CardDto>()
                    {
                        Succeeded = false,
                        Errors = new string[] { $"Status code: {StatusCodes.Status500InternalServerError}" },
                        Message = $"({ex.Message}) "
                    });
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
                if (_cache.TryGetValue(cacheKey, out CardDetailDto cachedCard))
                {
                    return cachedCard;
                }

                Card card = await _cardRepository.GetCardById(id);

                if (card == null)
                {
                    return NotFound(new Response<CardDetailDto>()
                    {
                        Errors = new string[] { "404" },
                        Message = "Card not found"
                    });
                }

                CardDetailDto cardDetailDto = _mapper.Map<CardDetailDto>(card);

                MemoryCacheEntryOptions cacheOptions = new MemoryCacheEntryOptions()
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
                };

                _cache.Set(cacheKey, cardDetailDto, cacheOptions);

                return Ok(cardDetailDto);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new Response<CardDetailDto>()
                    {
                        Succeeded = false,
                        Errors = new string[] { $"Status code: {StatusCodes.Status500InternalServerError}" },
                        Message = $"({ex.Message}) "
                    });
            }
        }
    }
}
