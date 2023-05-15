using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Howest.MagicCards.DAL.Models
{ 
    public class CardInDeck
    {
        [BsonElement("_id")]
        public int Id { get; set; }
        [BsonElement("count")]
        public int Count { get; set; }
    }
}
