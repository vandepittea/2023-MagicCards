namespace Howest.MagicCards.DAL.Repositories
{
    public interface IArtistRepository
    {
        Task<IQueryable<Artist>> GetArtists();
        Task<Artist> GetArtistById(long id, MtgDbContext dbContext = null);
    }
}
