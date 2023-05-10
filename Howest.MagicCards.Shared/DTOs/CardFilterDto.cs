using Howest.MagicCards.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Howest.MagicCards.Shared.DTOs
{
public class CardFilterDto : PaginationFilterDto
    {
        public string Set { get; set; }
        public string Artist { get; set; }
        public string Rarity { get; set; }
        public string CardType { get; set; }
        public string Name { get; set; }
        public string Text { get; set; }
        public SortDirection SortDirection { get; set; } = SortDirection.Ascending;
    }
    
    public abstract class PaginationFilterDto
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
