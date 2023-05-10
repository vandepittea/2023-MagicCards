using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Howest.MagicCards.Shared.DTOs;

namespace Howest.MagicCards.Shared
{
    public class CardFilter : PaginationFilter
    {
        public string SetName { get; set; }
        public string ArtistName { get; set; }
        public string Rarity { get; set; }
        public string Type { get; set; }
        public string SearchQuery { get; set; }
        public string SortBy { get; set; }
        public bool SortAscending { get; set; } = true;

        public CardFilter(CardFilterDto filterDto)
        {
            if (filterDto != null)
            {
                PageNumber = filterDto.PageNumber;
                PageSize = filterDto.PageSize;
                SetName = filterDto.SetName;
                ArtistName = filterDto.ArtistName;
                Rarity = filterDto.Rarity;
                Type = filterDto.Type;
                SearchQuery = filterDto.SearchQuery;
                SortBy = filterDto.SortBy;
                SortAscending = filterDto.SortAscending;
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
                throw new ArgumentException("Page size must be between 1 and 150.");
            }

            if (!string.IsNullOrEmpty(SetName) && SetName.Length > 50)
            {
                throw new ArgumentException("Set name cannot exceed 50 characters.");
            }

            if (!string.IsNullOrEmpty(ArtistName) && ArtistName.Length > 50)
            {
                throw new ArgumentException("Artist name cannot exceed 50 characters.");
            }

            if (!string.IsNullOrEmpty(Rarity) && Rarity.Length > 20)
            {
                throw new ArgumentException("Rarity cannot exceed 20 characters.");
            }

            if (!string.IsNullOrEmpty(Type) && Type.Length > 50)
            {
                throw new ArgumentException("Type cannot exceed 50 characters.");
            }

            if (!string.IsNullOrEmpty(SearchQuery) && SearchQuery.Length > 100)
            {
                throw new ArgumentException("Search query cannot exceed 100 characters.");
            }

            if (!string.IsNullOrEmpty(SortBy) && SortBy.Length > 50)
            {
                throw new ArgumentException("SortBy parameter cannot exceed 50 characters.");
            }
        }
    }
}
