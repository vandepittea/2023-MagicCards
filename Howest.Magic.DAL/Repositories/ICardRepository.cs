using Howest.MagicCards.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Howest.MagicCards.DAL.Repositories
{
    public interface ICardRepository
    {
        Task<IEnumerable<Card>> GetAllCards();
        Task<Card> GetCardById(int id);
        Task<IEnumerable<Artist>> GetAllArtists();
        Task<Artist> GetArtistById(int id);
        Task<IEnumerable<Card>> GetFilteredAndSortedCards(string setName = null, string artistName = null, string rarity = null, string type = null, string searchQuery = null, string sortBy = null, bool sortAscending = true, int? page = null, int? pageSize = null);
        Task<Card> AddCard(Card card);
        Task<Card> UpdateCard(Card card);
        Task DeleteCard(int id);
    }
}
