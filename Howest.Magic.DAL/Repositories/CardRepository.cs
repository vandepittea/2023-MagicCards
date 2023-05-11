using AutoMapper;
using AutoMapper.QueryableExtensions;
using Howest.MagicCards.DAL.Models;
using Howest.MagicCards.Shared;
using Howest.MagicCards.Shared.DTOs;
using Howest.MagicCards.Shared.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace Howest.MagicCards.DAL.Repositories
{
    public class CardRepository : ICardRepository
    {
        private readonly CardDbContext _context;
        private readonly IMapper _mapper;

        public CardRepository(IMapper mapper)
        {
            _context = new CardDbContext();
            _mapper = mapper;
        }

        public IQueryable<Card> ApplyCardFilter(CardParameterFilter filter)
        {
            filter.Validate();

            IQueryable<Card> query = _context.Cards
                .Include(c => c.Artist)
                .Include(c => c.SetCodeNavigation)
                .Include(c => c.RarityCodeNavigation)
                .Include(c => c.CardTypes)
                    .ThenInclude(ct => ct.Type)
                .Include(c => c.CardColors)
                    .ThenInclude(cc => cc.Color);

            if (!string.IsNullOrEmpty(filter.Set))
            {
                query = query.Where(c => c.SetCode == filter.Set);
            }

            if (!string.IsNullOrEmpty(filter.Artist))
            {
                query = query.Where(c => c.Artist.FullName.Contains(filter.Artist));
            }

            if (!string.IsNullOrEmpty(filter.Rarity))
            {
                query = query.Where(c => c.RarityCodeNavigation.Code == filter.Rarity);
            }

            if (!string.IsNullOrEmpty(filter.CardType))
            {
                query = query.Where(c => c.CardTypes.Any(ct => ct.Type.Name == filter.CardType));
            }

            if (!string.IsNullOrEmpty(filter.Name))
            {
                query = query.Where(c => c.Name.Contains(filter.Name));
            }

            if (!string.IsNullOrEmpty(filter.Text))
            {
                query = query.Where(c => c.Text.Contains(filter.Text));
            }

            return query;
        }

        public async Task<int> GetTotalCardCount(CardParameterFilter filter)
        {
            IQueryable<Card> query = ApplyCardFilter(filter);

            return await query.CountAsync();
        }

        public async Task<IEnumerable<CardDto>> GetCards(CardParameterFilter filter)
        {
            IQueryable<Card> query = ApplyCardFilter(filter);

            if (filter.SortDirection == SortDirection.Descending)
            {
                query = query.OrderByDescending(c => c.Name);
            }
            else
            {
                query = query.OrderBy(c => c.Name);
            }

            IQueryable<Card> pagedQuery = query.Skip((filter.PageNumber - 1) * filter.PageSize).Take(filter.PageSize);

            IEnumerable<CardDto> cards = await pagedQuery.ProjectTo<CardDto>(_mapper.ConfigurationProvider).ToListAsync();

            return cards;
        }
    }
}
