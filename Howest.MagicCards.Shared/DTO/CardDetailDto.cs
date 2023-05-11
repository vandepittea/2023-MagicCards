using Howest.MagicCards.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Howest.MagicCards.Shared.DTOs
{
    public class CardDetailDto : CardDto
    {
        public string Text { get; set; }
        public string Flavor { get; set; }
        public string Number { get; set; }
        public string Power { get; set; }
        public string Toughness { get; set; }
        public string Layout { get; set; }
        public int? MultiverseId { get; set; }
        public string OriginalImageUrl { get; set; }
        public string OriginalText { get; set; }
        public string OriginalType { get; set; }
        public string MtgId { get; set; }
        public string Variations { get; set; }
        public string ArtistName { get; set; }
    }
}
