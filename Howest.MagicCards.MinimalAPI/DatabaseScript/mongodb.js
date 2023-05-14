//Create database
use mtg_v1_deck

// Create deck collection
db.createCollection("deck");

// Add sample document to deck collection
db.deck.insertOne({
    _id: ObjectId(),
    cardId: ObjectId(),
    count: 1,
    createdAt: ISODate(),
    updatedAt: ISODate()
});