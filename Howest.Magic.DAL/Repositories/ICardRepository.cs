namespace Howest.MagicCards.DAL.Repositories
{
    public interface ICardRepository
    {
        Task<IEnumerable<Card>> GetCards();
        Task<Card> GetCardById(long id);
        Task<IEnumerable<Card>> GetCardsByArtistId(long artistId, MtgDbContext dbContext = null);
    }
}
