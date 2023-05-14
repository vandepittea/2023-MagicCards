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
            IMongoDatabase database = client.GetDatabase("mtg_v1_deck");
            _deck = database.GetCollection<CardInDeck>("deck");
        }

        public List<CardInDeck> GetCards()
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
            CardInDeck cardInDeck = _deck.Find(x => x.CardId == cardId).FirstOrDefault();
            if (cardInDeck != null)
            {
                cardInDeck.Count++;
                FilterDefinition<CardInDeck> filter = Builders<CardInDeck>.Filter.Eq(x => x.CardId, cardId);
                UpdateDefinition<CardInDeck> update = Builders<CardInDeck>.Update.Set(x => x.Count, cardInDeck.Count);
                _deck.UpdateOne(filter, update);
            }
            else
            {
                throw new ArgumentException("This card isn't part of the deck.");
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
                    FilterDefinition<CardInDeck> filter = Builders<CardInDeck>.Filter.Eq(x => x.CardId, cardId);
                    UpdateDefinition<CardInDeck> update = Builders<CardInDeck>.Update.Set(x => x.Count, cardInDeck.Count);
                    _deck.UpdateOne(filter, update);
                }
            }
            else
            {
                throw new ArgumentException("This card isn't part of the deck.");
            }
        }

        public void ClearDeck()
        {
            _deck.DeleteMany(new BsonDocument());
        }
    }
}