using System.Threading.Tasks;

namespace Howest.MagicCards.DAL.Repositories
{
    public interface IArtistRepository
    {
        Task<Artist> GetArtistById(long id);
    }
}