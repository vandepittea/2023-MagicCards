//Create database
use mtg_v1

// Create artists collection
db.createCollection("artists");
db.artists.insertOne({
    _id: ObjectId(),
    fullName: "John Smith",
    createdAt: ISODate(),
    updatedAt: ISODate()
});

// Create colors collection
db.createCollection("colors");
db.colors.insertMany([
    {
        _id: ObjectId(),
        name: "Red",
        createdAt: ISODate(),
        updatedAt: ISODate()
    },
    {
        _id: ObjectId(),
        name: "Green",
        createdAt: ISODate(),
        updatedAt: ISODate()
    },
    {
        _id: ObjectId(),
        name: "Blue",
        createdAt: ISODate(),
        updatedAt: ISODate()
    }
]);

// Create rarities collection
db.createCollection("rarities");
db.rarities.insertMany([
    {
        _id: ObjectId(),
        name: "Common",
        createdAt: ISODate(),
        updatedAt: ISODate()
    },
    {
        _id: ObjectId(),
        name: "Uncommon",
        createdAt: ISODate(),
        updatedAt: ISODate()
    },
    {
        _id: ObjectId(),
        name: "Rare",
        createdAt: ISODate(),
        updatedAt: ISODate()
    }
]);

// Create sets collection
db.createCollection("sets");
db.sets.insertMany([
    {
        _id: ObjectId(),
        name: "Zendikar Rising",
        code: "ZNR",
        createdAt: ISODate(),
        updatedAt: ISODate()
    },
    {
        _id: ObjectId(),
        name: "Core Set 2021",
        code: "M21",
        createdAt: ISODate(),
        updatedAt: ISODate()
    }
]);

// Create types collection
db.createCollection("types");
db.types.insertMany([
    {
        _id: ObjectId(),
        name: "Creature",
        createdAt: ISODate(),
        updatedAt: ISODate()
    },
    {
        _id: ObjectId(),
        name: "Enchantment",
        createdAt: ISODate(),
        updatedAt: ISODate()
    },
    {
        _id: ObjectId(),
        name: "Instant",
        createdAt: ISODate(),
        updatedAt: ISODate()
    },
    {
        _id: ObjectId(),
        name: "Sorcery",
        createdAt: ISODate(),
        updatedAt: ISODate()
    }
]);

// Create cards collection
db.createCollection("cards");
db.cards.insertOne({
    _id: ObjectId(),
    name: "Example Card",
    manaCost: "{X}{Y}",
    type: "Creature - Example",
    setCode: "EX",
    rarityCode: "C",
    image: "https://example.com/cardimage.jpg",
    cardColors: ["Red", "Blue"],
    cardTypes: ["Creature", "Artifact"],
    text: "Example text.",
    flavor: "Example flavor text.",
    number: "001",
    power: "3",
    toughness: "3",
    layout: "normal",
    multiverseId: 123456,
    originalImageUrl: "https://example.com/originalimage.jpg",
    originalText: "Example original text.",
    originalType: "Example original type.",
    mtgId: "123abc",
    variations: ["456def", "789ghi"],
    artistId: ObjectId(),
    createdAt: ISODate(),
    updatedAt: ISODate()
});