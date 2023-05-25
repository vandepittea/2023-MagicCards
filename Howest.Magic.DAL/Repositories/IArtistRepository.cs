using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Howest.MagicCards.DAL.Repositories
{
    public interface IArtistRepository
    {
        Task<IQueryable<Artist>> GetArtists();
        Task<Artist> GetArtistById(long id, MtgDbContext dbContext = null);
    }
}
