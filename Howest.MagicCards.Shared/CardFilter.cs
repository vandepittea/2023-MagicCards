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

        public CardFilter(CardFilterDto filterDto)
        {
            Set = filterDto.Set;
            Artist = filterDto.Artist;
            Rarity = filterDto.Rarity;
            CardType = filterDto.CardType;
            Name = filterDto.Name;
            Text = filterDto.Text;
            SortDirection = filterDto.SortDirection;
        }

        public IQueryable<CardFilterDto> ApplyFilter(IQueryable<CardFilterDto> query)
        {
            if (!string.IsNullOrEmpty(Set))
            {
                query = query.Where(c => c.SetCode == Set);
            }

            if (!string.IsNullOrEmpty(Artist))
            {
                query = query.Where(c => c.Artist.FullName.Contains(Artist));
            }

            if (!string.IsNullOrEmpty(Rarity))
            {
                query = query.Where(c => c.RarityCodeNavigation.Code == Rarity);
            }

            if (!string.IsNullOrEmpty(CardType))
            {
                query = query.Where(c => c.CardTypes.Any(ct => ct.Type.Name == CardType));
            }

            if (!string.IsNullOrEmpty(Name))
            {
                query = query.Where(c => c.Name.Contains(Name));
            }

            if (!string.IsNullOrEmpty(Text))
            {
                query = query.Where(c => c.Text.Contains(Text));
            }

            return SortDirection == SortDirection.Ascending
                ? query.OrderBy(c => c.Name)
                : query.OrderByDescending(c => c.Name);
        }
    }
}
