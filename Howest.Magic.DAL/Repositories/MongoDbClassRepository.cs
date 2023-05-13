using Howest.MagicCards.DAL.Models;
using MongoDB.Driver;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Howest.MagicCards.DAL.Repositories
{
    public class MongoDbCardRepository : ICardRepository
    {
        private readonly IMongoCollection<Card> _cards;

        public MongoDbCardRepository()
        {
            MongoClient client = new();
            IMongoDatabase database = client.GetDatabase("mtg_v1");
            _cards = database.GetCollection<Card>("cards");
        }

        public void AddCard(Card newCard)
        {
            _cards.InsertOne(newCard);
        }

        public void UpdateCard(Card updatedCard, string id)
        {
            FilterDefinition<Card> filter = Builders<Card>.Filter.Eq("_id", ObjectId.Parse(id));
            _cards.ReplaceOne(filter, updatedCard);
        }

        public void DeleteCard(string id)
        {
            FilterDefinition<Card> filter = Builders<Card>.Filter.Eq("_id", ObjectId.Parse(id));
            _cards.DeleteOne(filter);
        }

        public Task<IEnumerable<Card>> GetCards()
        {
            throw new NotImplementedException();
        }

        public Task<Card> GetCardById(int id)
        {
            throw new NotImplementedException();
        }
    }
}
