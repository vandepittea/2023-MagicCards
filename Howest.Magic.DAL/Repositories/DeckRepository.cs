using Howest.MagicCards.DAL.Models;
using MongoDB.Driver;
using MongoDB.Bson;
using System.Collections.Generic;
using System.Linq;

namespace Howest.MagicCards.DAL.Repositories
{
    public class DeckRepository : IDeckRepository
    {
        private readonly IMongoCollection<CardInDeck> _deck;

        public DeckRepository()
        {
            MongoClient client = new MongoClient();
            IMongoDatabase database = client.GetDatabase("mtg_v1");
            _deck = database.GetCollection<CardInDeck>("deck");
        }

        public List<CardInDeck> GetDeck()
        {
            return _deck.Find(new BsonDocument()).ToList();
        }

        public void AddCardToDeck(int cardId)
        {
            var cardInDeck = _deck.Find(x => x.CardId == cardId).FirstOrDefault();
            if (cardInDeck == null)
            {
                _deck.InsertOne(new CardInDeck { CardId = cardId, Count = 1 });
            }
            else
            {
                IncrementCardCount(cardId);
            }
        }

        public void IncrementCardCount(int cardId)
        {
            var cardInDeck = _deck.Find(x => x.CardId == cardId).FirstOrDefault();
            if (cardInDeck != null)
            {
                cardInDeck.Count++;
            }
        }

        public void RemoveCardFromDeck(int cardId)
        {
            var cardInDeck = _deck.Find(x => x.CardId == cardId).FirstOrDefault();
            if (cardInDeck != null)
            {
                if (cardInDeck.Count == 1)
                {
                    _deck.DeleteOne(x => x.CardId == cardId);
                }
                else
                {
                    cardInDeck.Count--;
                }
            }
        }

        public void ClearDeck()
        {
            _deck.DeleteMany(new BsonDocument());
        }
    }
}