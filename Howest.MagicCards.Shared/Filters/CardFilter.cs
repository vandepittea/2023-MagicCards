namespace Howest.MagicCards.Shared.Filters
{
    public class CardFilter : PaginationFilter
    {
        public string SetCode { get; set; }
        public string ArtistName { get; set; }
        public string RarityCode { get; set; }
        public string CardType { get; set; }
        public string CardName { get; set; }
        public string CardText { get; set; }
    }
}
