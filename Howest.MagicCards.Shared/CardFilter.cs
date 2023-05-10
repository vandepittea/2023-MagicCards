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
            if (filterDto != null)
            {
                PageNumber = filterDto.PageNumber;
                PageSize = filterDto.PageSize;
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

    public class CardParameterFilter : CardFilter
    {
        private new readonly int _maxPageSize = PaginationFilter._maxPageSize;

        public CardParameterFilter(CardFilterDto filterDto) : base(filterDto)
        {
        }

        public void Validate()
        {
            if (PageNumber < 1)
            {
                throw new ArgumentException("Page number cannot be less than 1.");
            }

            if (PageSize < 1 || PageSize > _maxPageSize)
            {
                throw new ArgumentException($"Page size must be between 1 and {_maxPageSize}.");
            }

            if (!string.IsNullOrEmpty(Set) && Set.Length > 50)
            {
                throw new ArgumentException("Set name cannot exceed 50 characters.");
            }

            if (!string.IsNullOrEmpty(Artist) && Artist.Length > 50)
            {
                throw new ArgumentException("Artist name cannot exceed 50 characters.");
            }

            if (!string.IsNullOrEmpty(Rarity) && Rarity.Length > 20)
            {
                throw new ArgumentException("Rarity cannot exceed 20 characters.");
            }

            if (!string.IsNullOrEmpty(CardType) && CardType.Length > 50)
            {
                throw new ArgumentException("Card type cannot exceed 50 characters.");
            }

            if (!string.IsNullOrEmpty(Name) && Name.Length > 200)
            {
                throw new ArgumentException("Card name cannot exceed 200 characters.");
            }

            if (!string.IsNullOrEmpty(Text) && Text.Length > 500)
            {
                throw new ArgumentException("Card text cannot exceed 500 characters.");
            }
        }
    }
}
