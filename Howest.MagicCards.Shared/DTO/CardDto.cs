using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Howest.MagicCards.Shared.DTOs
{
    public class CardDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string ManaCost { get; set; }
        public string Type { get; set; }
        public string SetCode { get; set; }
        public string RarityCode { get; set; }
        public string Image { get; set; }
        public IEnumerable<string> CardColors { get; set; }
        public IEnumerable<string> CardTypes { get; set; }
    }
}
