namespace Howest.MagicCards.DAL.Repositories
{
    public interface ICardRepository
    {
        Task<IEnumerable<Card>> GetCards();
        Task<Card> GetCardById(long id);
        Task<Artist> GetArtistById(long id);
        Task<IEnumerable<Card>> GetCardsByArtistId(long artistId);
    }
}
