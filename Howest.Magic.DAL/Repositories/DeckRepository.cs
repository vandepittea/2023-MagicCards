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

        public void AddCardToDeck(CardInDeck cardInDeck)
        {
            CardInDeck foundCard = _deck.Find(x => x.Id == cardInDeck.Id).FirstOrDefault();
            if (foundCard == null)
            {
                _deck.InsertOne(cardInDeck);
            }
            else
            {
                throw new ArgumentException("This card is already in the deck");
            }
        }

        public void IncrementCardCount(CardInDeck cardInDeck)
        {
            CardInDeck foundCard = _deck.Find(x => x.Id == cardInDeck.Id).FirstOrDefault();
            if (foundCard != null)
            {
                foundCard.Count++;
                FilterDefinition<CardInDeck> filter = Builders<CardInDeck>.Filter.Eq(x => x.Id, foundCard.Id);
                UpdateDefinition<CardInDeck> update = Builders<CardInDeck>.Update.Set(x => x.Count, foundCard.Count);
                _deck.UpdateOne(filter, update);
            }
            else
            {
                throw new ArgumentException("This card isn't part of the deck");
            }
        }

        public void RemoveCardFromDeck(int cardId)
        {
            var cardInDeck = _deck.Find(x => x.Id == cardId).FirstOrDefault();
            if (cardInDeck != null)
            {
                if (cardInDeck.Count == 1)
                {
                    _deck.DeleteOne(x => x.Id == cardId);
                }
                else
                {
                    cardInDeck.Count--;
                    FilterDefinition<CardInDeck> filter = Builders<CardInDeck>.Filter.Eq(x => x.Id, cardId);
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