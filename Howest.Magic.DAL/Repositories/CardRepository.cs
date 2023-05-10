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

        public CardRepository()
        {
            _context = new CardDbContext();
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

            var pagedQuery = query.Skip((filter.PageNumber - 1) * filter.PageSize).Take(filter.PageSize);

            var cards = await pagedQuery.Select(c => new CardDto
            {
                Id = c.Id,
                Name = c.Name,
                ManaCost = c.ManaCost,
                ConvertedManaCost = c.ConvertedManaCost,
                Type = c.Type,
                Rarity = c.RarityCodeNavigation.Name,
                Set = c.SetCodeNavigation.Name,
                Text = c.Text,
                Flavor = c.Flavor,
                ArtistName = c.Artist.FullName,
                Number = c.Number,
                Power = c.Power,
                Toughness = c.Toughness,
                Layout = c.Layout,
                MultiverseId = c.MultiverseId,
                OriginalImageUrl = c.OriginalImageUrl,
                Image = c.Image,
                OriginalText = c.OriginalText,
                OriginalType = c.OriginalType,
                MtgId = c.MtgId,
                Variations = c.Variations,
                Colors = c.CardColors.Select(cc => cc.Color.Name).ToList(),
                Types = c.CardTypes.Select(ct => ct.Type.Name).ToList()
            }).ToListAsync();

            return cards;
        }
    }
}
