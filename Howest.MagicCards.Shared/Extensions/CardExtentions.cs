using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Howest.MagicCards.Shared.Extensions
{
    public static class CardExtensions
    {
        public static IQueryable<Card> FilterBySet(this IQueryable<Card> cards, string setName)
        {
            if (!string.IsNullOrEmpty(setName))
            {
                cards = cards.Where(c => c.SetCodeNavigation.Name.Contains(setName));
            }

            return cards;
        }

        public static IQueryable<Card> FilterByArtist(this IQueryable<Card> cards, string artistName)
        {
            if (!string.IsNullOrEmpty(artistName))
            {
                cards = cards.Where(c => c.Artist.FullName.ToLower().Contains(artistName.ToLower()));
            }

            return cards;
        }

        public static IQueryable<Card> FilterByRarity(this IQueryable<Card> cards, string rarityName)
        {
            if (!string.IsNullOrEmpty(rarityName))
            {
                cards = cards.Where(c => c.RarityCodeNavigation.Name.Contains(rarityName));
            }

            return cards;
        }

        public static IQueryable<Card> FilterByCardType(this IQueryable<Card> cards, string typeName)
        {
            if (!string.IsNullOrEmpty(typeName))
            {
                cards = cards.Where(c => c.CardTypes.Any(ct => ct.Type.Name.ToLower().Contains(typeName.ToLower())));
            }

            return cards;
        }

        public static IQueryable<Card> FilterByCardName(this IQueryable<Card> cards, string cardName)
        {
            if (!string.IsNullOrEmpty(cardName))
            {
                cards = cards.Where(c => c.Name.ToLower().Contains(cardName.ToLower()));
            }

            return cards;
        }

        public static IQueryable<Card> FilterByCardText(this IQueryable<Card> cards, string cardText)
        {
            if (!string.IsNullOrEmpty(cardText))
            {
                cards = cards.Where(c => c.Text.ToLower().Contains(cardText.ToLower()));
            }

            return cards;
        }

        public static IQueryable<Card> Sort(this IQueryable<Card> cards, string sortOrder)
        {
            if (string.IsNullOrEmpty(sortOrder))
            {
                return cards.OrderBy(c => c.Name);
            }

            sortOrder = sortOrder.Trim().ToLower();
            bool descending = sortOrder.EndsWith("desc");

            return descending
                ? cards.OrderByDescending(c => c.Name)
                : cards.OrderBy(c => c.Name);
        }
    }
}
