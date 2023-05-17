namespace Howest.MagicCards.DAL.Repositories
{
    public interface ICardRepository
    {
        Task<IQueryable<Card>> GetCards();
        Task<Card> GetCardById(long id);
        Task<IEnumerable<Card>> GetCardsByArtistId(long artistId, MtgDbContext dbContext = null);
    }
}
