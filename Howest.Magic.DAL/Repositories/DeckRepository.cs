using Howest.MagicCards.DAL.Models;
using MongoDB.Driver;
using MongoDB.Bson;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;

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

        public void AddCardToDeck(CardInDeck cardInDeck, int maxCardsInDeck)
        {
            List<CardInDeck> deckCards = GetCards();
            if (deckCards.Count >= maxCardsInDeck)
            {
                throw new InvalidOperationException("The deck is already full. Cannot add more cards.");
            }

            CardInDeck foundCard = deckCards.FirstOrDefault(x => x.Id == cardInDeck.Id);
            if (foundCard == null)
            {
                _deck.InsertOne(cardInDeck);
            }
            else
            {
                throw new ArgumentException("This card is already in the deck");
            }
        }

        public void UpdateCardCount(CardInDeck cardInDeck)
        {
            CardInDeck foundCard = _deck.Find(x => x.Id == cardInDeck.Id).FirstOrDefault();
            if (foundCard != null)
            {
                FilterDefinition<CardInDeck> filter = Builders<CardInDeck>.Filter.Eq(x => x.Id, foundCard.Id);
                UpdateDefinition<CardInDeck> update = Builders<CardInDeck>.Update.Set(x => x.Count, cardInDeck.Count);
                _deck.UpdateOne(filter, update);
            }
            else
            {
                throw new ArgumentException("This card isn't part of the deck");
            }
        }

        public void RemoveCardFromDeck(int cardId)
        {
            CardInDeck cardInDeck = _deck.Find(x => x.Id == cardId).FirstOrDefault();
            if (cardInDeck != null)
            {
                _deck.DeleteOne(x => x.Id == cardId);
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