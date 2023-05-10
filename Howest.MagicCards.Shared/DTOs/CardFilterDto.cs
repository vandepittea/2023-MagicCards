using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Howest.MagicCards.Shared.DTOs
{
    public class CardFilterDto : PaginationFilterDto
    {
        public string SetName { get; set; }
        public string ArtistName { get; set; }
        public string Rarity { get; set; }
        public string Type { get; set; }
        public string SearchQuery { get; set; }
        public string SortBy { get; set; }
        public bool SortAscending { get; set; } = true;
    }

    public abstract class PaginationFilterDto
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}
