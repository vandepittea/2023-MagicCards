using Microsoft.EntityFrameworkCore;

namespace Howest.MagicCards.DAL.Repositories
{
    public class ArtistRepository : IArtistRepository
    {
        private readonly MtgDbContext _dbContext;

        public ArtistRepository(MtgDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Artist> GetArtistById(long id)
        {
            return await _dbContext.Artists.FindAsync(id);
        }

        public async Task<IEnumerable<Artist>> GetArtists()
        {
            return await _dbContext.Artists.ToListAsync();
        }
    }
}
