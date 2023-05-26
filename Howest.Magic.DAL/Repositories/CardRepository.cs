using Microsoft.EntityFrameworkCore;

namespace Howest.MagicCards.DAL.Repositories
{
    public class CardRepository : ICardRepository
    {
        private readonly MtgDbContext _dbContext;

        public CardRepository(MtgDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IQueryable<Card>> GetCards()
        {
            IQueryable<Card> cardsQuery = _dbContext.Cards
                .Include(c => c.SetCodeNavigation)
                .Include(c => c.RarityCodeNavigation)
                .Include(c => c.CardColors)
                    .ThenInclude(cc => cc.Color)
                .Include(c => c.CardTypes)
                    .ThenInclude(ct => ct.Type);

                return await Task.FromResult(cardsQuery);
        }

        public async Task<Card> GetCardById(long id)
        {
            return await _dbContext.Cards
                .Include(c => c.SetCodeNavigation)
                .Include(c => c.RarityCodeNavigation)
                .Include(c => c.CardColors)
                    .ThenInclude(cc => cc.Color)
                .Include(c => c.CardTypes)
                    .ThenInclude(ct => ct.Type)
                .Include(c => c.Artist)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<IEnumerable<Card>> GetCardsByArtistId(long artistId, MtgDbContext dbContext = null)
        {
            IQueryable<Card> query;

            if (dbContext == null)
            {
                query = _dbContext.Cards;
            }
            else
            {
                query = dbContext.Cards;
            }

            query = query
                .Include(c => c.SetCodeNavigation)
                .Include(c => c.RarityCodeNavigation)
                .Include(c => c.CardColors).ThenInclude(cc => cc.Color)
                .Include(c => c.CardTypes).ThenInclude(ct => ct.Type)
                .Where(c => c.ArtistId == artistId);

            return await query.ToListAsync();
        }
    }
}
