using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Howest.MagicCards.Shared.Filters
{
    public class ArtistFilter : PaginationFilter
    {
        public int Limit { get; set; }
    }
}
