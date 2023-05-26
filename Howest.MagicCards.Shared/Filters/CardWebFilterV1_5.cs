namespace Howest.MagicCards.Shared.Filters
{
    public class CardWebFilterV1_5 : PaginationFilterV1_5
    {
        public string SetName { get; set; }
        public string ArtistName { get; set; }
        public string RarityName { get; set; }
        public string TypeName { get; set; }
        public string CardName { get; set; }
        public string CardText { get; set; }
    }
}