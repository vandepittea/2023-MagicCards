using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Howest.MagicCards.Shared.DTOs;
using Howest.MagicCards.Shared.Enums;

namespace Howest.MagicCards.Shared
{
    public class CardFilter : PaginationFilter
    {
        public string Set { get; set; }
        public string Artist { get; set; }
        public string Rarity { get; set; }
        public string CardType { get; set; }
        public string Name { get; set; }
        public string Text { get; set; }
        public SortDirection SortDirection { get; set; } = SortDirection.Ascending;

        public CardFilter(CardDetailDto filterDto)
        {
            Set = filterDto.Set;
            Artist = filterDto.Artist;
            Rarity = filterDto.Rarity;
            CardType = filterDto.CardType;
            Name = filterDto.Name;
            Text = filterDto.Text;
            SortDirection = filterDto.SortDirection;
        }
    }
}
