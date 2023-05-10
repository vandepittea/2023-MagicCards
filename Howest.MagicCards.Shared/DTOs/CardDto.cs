using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Howest.MagicCards.Shared.DTOs
{
    public class CardDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string SetName { get; set; }
        public string ArtistName { get; set; }
        public string Rarity { get; set; }
        public string Type { get; set; }
    }
}
