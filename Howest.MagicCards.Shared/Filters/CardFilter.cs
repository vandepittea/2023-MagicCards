namespace Howest.MagicCards.Shared.Filters
{
    public class CardFilter : PaginationFilter
    {
        public string Power { get; set; }
        public string Toughness { get; set; }
        public string SetName { get; set; }
        public string ArtistName { get; set; }
        public string RarityName { get; set; }
        public string TypeName { get; set; }
        public string CardName { get; set; }
        public string CardText { get; set; }
    }
}
