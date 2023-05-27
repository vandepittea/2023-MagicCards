using System.ComponentModel.DataAnnotations;

namespace Howest.MagicCards.Shared.Filters
{
    public class CardWebFilterV1_5 : PaginationFilterV1_5
    {
        [StringLength(10, ErrorMessage = "Maximum 10 characters allowed for Set Name.")]
        public string SetName { get; set; }

        [StringLength(10, ErrorMessage = "Maximum 10 characters allowed for Artist Name.")]
        public string ArtistName { get; set; }

        [StringLength(10, ErrorMessage = "Maximum 10 characters allowed for Rarity Name.")]
        public string RarityName { get; set; }

        [StringLength(10, ErrorMessage = "Maximum 10 characters allowed for Type Name.")]
        public string TypeName { get; set; }

        [StringLength(10, ErrorMessage = "Maximum 10 characters allowed for Card Name.")]
        public string CardName { get; set; }

        [StringLength(10, ErrorMessage = "Maximum 10 characters allowed for Card Text.")]
        public string CardText { get; set; }
    }
}