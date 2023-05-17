using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;

namespace Howest.MagicCards.Shared.Filters
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class PaginationFilter
    {
        private int _pageSize;
        private int _pageNumber = 1;

        [JsonIgnore]
        public int MaxPageSize { get; set; }

        public int PageNumber
        {
            get { return _pageNumber; }
            set { _pageNumber = value < 1 ? 1 : value; }
        }

        public int PageSize
        {
            get { return _pageSize > MaxPageSize ? MaxPageSize : _pageSize; }
            set { _pageSize = value > MaxPageSize || value < 1 ? MaxPageSize : value; }
        }

        public string SortBy { get; set; }
    }

}
