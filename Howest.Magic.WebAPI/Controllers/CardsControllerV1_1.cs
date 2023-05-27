namespace Howest.MagicCards.WebAPI.Controllers.V1_1
{
    [ApiVersion("1.1")]
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
        public async Task<ActionResult<PagedResponse<IQueryable<CardDto>>>> GetCards([FromQuery] CardWebFilterV1_1 filter, [FromServices] IConfiguration config)
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

        private string GetCacheKey(CardWebFilterV1_1 filter)
        {
            return $"cards_{JsonSerializer.Serialize(filter)}";
        }

        private IQueryable<Card> FilterCards(IQueryable<Card> cards, CardWebFilterV1_1 filter)
        {
            return cards
                .FilterBySet(filter.SetName)
                .FilterByArtist(filter.ArtistName)
                .FilterByRarity(filter.RarityName)
                .FilterByCardType(filter.TypeName)
                .FilterByCardName(filter.CardName)
                .FilterByCardText(filter.CardText);
        }

        private IEnumerable<CardDto> GetPagedCards(IQueryable<Card> cards, CardWebFilterV1_1 filter, IConfiguration config)
        {
            return cards
                .ToPagedList<Card>(filter.PageNumber, filter.PageSize, int.Parse(config.GetSection("appSettings")["minPageSize"]), int.Parse(config.GetSection("appSettings")["maxPageSize"]))
                .ProjectTo<CardDto>(_mapper.ConfigurationProvider)
                .ToList();
        }
    }
}