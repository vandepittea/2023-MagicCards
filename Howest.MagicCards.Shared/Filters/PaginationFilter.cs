using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;

namespace Howest.MagicCards.Shared.Filters
{
    public class PaginationFilter
    {
        public int PageNumber
        { get; set; }

        public int PageSize
        {
            get; set;
        }

        public string SortBy { get; set; }
    }
}
