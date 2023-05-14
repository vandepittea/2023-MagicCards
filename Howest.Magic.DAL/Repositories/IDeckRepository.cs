using Howest.MagicCards.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Howest.MagicCards.DAL.Repositories
{
    public interface IDeckRepository
    {
        List<CardInDeck> GetCards();
        void AddCardToDeck(int cardId);
        void IncrementCardCount(int cardId);
        void RemoveCardFromDeck(int cardId);
        void ClearDeck();
    }
}
