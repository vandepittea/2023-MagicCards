using AutoMapper;
using AutoMapper.QueryableExtensions;
using Howest.MagicCards.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace Howest.MagicCards.DAL.Repositories
{
    public class CardRepository : ICardRepository
    {
        private readonly MtgDbContext _dbContext;

        public CardRepository(MtgDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Card>> GetCards()
        {
            return await _dbContext.Cards
                .Include(c => c.SetCodeNavigation)
                .Include(c => c.RarityCodeNavigation)
                .Include(c => c.CardColors)
                    .ThenInclude(cc => cc.Color)
                .Include(c => c.CardTypes)
                    .ThenInclude(ct => ct.Type)
                .Select(c => c)
                .ToListAsync();
        }

        public async Task<Card> GetCardById(int id)
        {
            return await _dbContext.Cards
                .Include(c => c.SetCodeNavigation)
                .Include(c => c.RarityCodeNavigation)
                .Include(c => c.CardColors)
                    .ThenInclude(cc => cc.Color)
                .Include(c => c.CardTypes)
                    .ThenInclude(ct => ct.Type)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

    }
}
