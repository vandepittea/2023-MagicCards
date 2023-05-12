using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Howest.MagicCards.DAL.Repositories;
using Howest.MagicCards.DAL.Models;
using Howest.MagicCards.Shared.DTOs;
using FluentValidation;
using Howest.MagicCards.Shared.Validation;
using AutoMapper;
using Howest.MagicCards.Shared.Filters;
using Howest.MagicCards.Shared.Extensions;
using AutoMapper.QueryableExtensions;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using StackExchange.Redis;
using Microsoft.Extensions.Caching.Memory;

namespace Howest.MagicCards.WebAPI.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class CardsController : ControllerBase
    {
        private readonly IDistributedCache _cache;
        private readonly CardRepository _cardRepository;
        private readonly IMapper _mapper;

        public CardsController(CardRepository cardRepository, IMapper mapper, IDistributedCache memoryCache)
        {
            _cardRepository = cardRepository;
            _mapper = mapper;
            _cache = memoryCache;
        }

        [HttpGet]
        [ProducesResponseType(typeof(CardDto), 200)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 500)]
        public async Task<ActionResult<PagedResponse<IQueryable<CardDto>>>> GetCards([FromQuery] CardFilter filter,
            [FromServices] IConfiguration config)
        {
            try
            {
                filter.MaxPageSize = int.Parse(config["maxPageSize"]);

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

                PagedResponse<CardDto> response = new PagedResponse<CardDto>(pagedCards, filter.PageNumber, filter.PageSize, totalCount, totalPages);
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
        private readonly IDistributedCache _cache;
        private readonly CardRepository _cardRepository;
        private readonly IMapper _mapper;

        public CardsController(CardRepository cardRepository, IMapper mapper, IDistributedCache memoryCache)
        {
            _cardRepository = cardRepository;
            _mapper = mapper;
            _cache = memoryCache;
        }

        [HttpGet]
        [ProducesResponseType(typeof(CardDto), 200)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 500)]
        public async Task<ActionResult<PagedResponse<IQueryable<CardDto>>>> GetCards([FromQuery] CardFilter filter,
            [FromServices] IConfiguration config)
        {
            try
            {
                filter.MaxPageSize = int.Parse(config["maxPageSize"]);

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
                    .FilterByCardText(filter.CardText)
                    .Sort(filter.SortBy);

                IEnumerable<CardDto> pagedCards = filteredCards
                    .ToPagedList<Card>(filter.PageNumber, filter.PageSize)
                    .ProjectTo<CardDto>(_mapper.ConfigurationProvider)
                    .ToList();

                int totalCount = filteredCards.Count();
                int totalPages = (int)Math.Ceiling(totalCount / (double)filter.PageSize);

                PagedResponse<CardDto> response = new PagedResponse<CardDto>(pagedCards, filter.PageNumber, filter.PageSize, totalCount, totalPages);
                string responseData = JsonSerializer.Serialize(response);

                DistributedCacheEntryOptions cacheOptions = new DistributedCacheEntryOptions()
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
                };

                await _cache.SetStringAsync(cacheKey, responseData, cacheOptions);

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
                string cachedData = await _cache.GetStringAsync(cacheKey);

                if (cachedData != null)
                {
                    CardDetailDto cachedCard = JsonSerializer.Deserialize<CardDetailDto>(cachedData);
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
                string responseData = JsonSerializer.Serialize(cardDetailDto);

                DistributedCacheEntryOptions cacheOptions = new DistributedCacheEntryOptions()
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
                };

                await _cache.SetStringAsync(cacheKey, responseData, cacheOptions);

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
