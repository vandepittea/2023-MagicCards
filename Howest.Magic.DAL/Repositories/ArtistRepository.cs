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

        public async Task<IEnumerable<Artist>> GetArtists()
        {
            return await _dbContext.Artists.ToListAsync();
        }

        public async Task<Artist> GetArtistById(long id, MtgDbContext dbContext = null)
        {
            MtgDbContext context = dbContext ?? _dbContext;
            return await context.Artists.FindAsync(id);
        }
    }
}
