﻿using Howest.MagicCards.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Howest.MagicCards.DAL.Repositories
{
    public interface ICardRepository
    {
        Task<IEnumerable<Card>> GetCards();
        Task<Card> GetCardById(int id);
        void AddCard(Card newCard);
        void UpdateCard(Card updatedCard, string id);
        void DeleteCard(string id);
    }
}
