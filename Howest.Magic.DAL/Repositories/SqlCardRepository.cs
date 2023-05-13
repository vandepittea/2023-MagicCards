using Howest.MagicCards.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace Howest.MagicCards.DAL.Repositories
{
    public class SqlCardRepository : ICardRepository
    {
        private readonly MtgDbContext _dbContext;

        public SqlCardRepository(MtgDbContext dbContext)
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

        public void AddCard(Card newCard)
        {
            throw new NotImplementedException();
        }

        public void UpdateCard(Card updatedCard, string id)
        {
            throw new NotImplementedException();
        }

        public void DeleteCard(string id)
        {
            throw new NotImplementedException();
        }
    }
}
