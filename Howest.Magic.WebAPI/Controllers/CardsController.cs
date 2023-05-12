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


namespace Howest.MagicCards.WebAPI.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class CardsController : ControllerBase
    {
        private readonly CardRepository _cardRepository;
        private readonly IMapper _mapper;

        public CardsController(CardRepository cardRepository, IMapper mapper)
        {
            _cardRepository = cardRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(typeof(CardDto), 200)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 500)]
        public async Task<ActionResult<PagedResponse<IQueryable<CardDto>>>> GetCards([FromQuery] CardFilter filter,
            [FromQuery] bool sortOrder,
            [FromServices] IConfiguration config)
        {
            try
            {
                filter.MaxPageSize = int.Parse(config["maxPageSize"]);

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

                return Ok(new PagedResponse<CardDto>(pagedCards, filter.PageNumber, filter.PageSize, totalCount, totalPages));
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
        private readonly CardRepository _cardRepository;
        private readonly IMapper _mapper;

        public CardsController(CardRepository cardRepository, IMapper mapper)
        {
            _cardRepository = cardRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(typeof(CardDto), 200)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 500)]
        public async Task<ActionResult<PagedResponse<IQueryable<CardDto>>>> GetCards([FromQuery] CardFilter filter,
            [FromQuery] bool sortOrder,
            [FromServices] IConfiguration config)
        {
            try
            {
                filter.MaxPageSize = int.Parse(config["maxPageSize"]);

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

                return Ok(new PagedResponse<CardDto>(pagedCards, filter.PageNumber, filter.PageSize, totalCount, totalPages));
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
    }
}
