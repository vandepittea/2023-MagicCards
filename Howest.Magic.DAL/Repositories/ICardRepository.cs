using Howest.MagicCards.DAL.Models;
using Howest.MagicCards.Shared;
using Howest.MagicCards.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Howest.MagicCards.DAL.Repositories
{
    public interface ICardRepository
    {
        Task<IEnumerable<CardDto>> GetCards(CardParameterFilter filter);
        Task<int> GetTotalCardCount(CardParameterFilter filter);
    }
}
