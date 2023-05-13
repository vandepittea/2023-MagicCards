using Microsoft.Extensions.Configuration;

namespace Howest.MagicCards.Shared.Filters
{
    public class CardFilter : PaginationFilter
    {
        public CardFilter(IConfiguration config) : base(config)
        {
        }

        public string SetCode { get; set; }
        public string ArtistName { get; set; }
        public string RarityCode { get; set; }
        public string CardType { get; set; }
        public string CardName { get; set; }
        public string CardText { get; set; }
    }
}
