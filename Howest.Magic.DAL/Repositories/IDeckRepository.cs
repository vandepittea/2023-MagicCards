namespace Howest.MagicCards.DAL.Repositories
{
    public interface IDeckRepository
    {
        List<CardInDeck> GetCards();
        void AddCardToDeck(CardInDeck cardInDeck, int maxCardsInDeck);
        void UpdateCardCount(CardInDeck cardInDeck);
        void RemoveCardFromDeck(int cardId);
        void ClearDeck();
    }
}
