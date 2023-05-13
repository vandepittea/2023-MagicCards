using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Howest.MagicCards.Shared.Filters
{
    public class PaginationFilter
    {
        private readonly int _maxPageSize;

        private int _pageSize;
        private int _pageNumber = 1;

        public PaginationFilter(IConfiguration config)
        {
            _maxPageSize = int.Parse(config.GetSection("appSettings")["maxPageSize"]);
        }

        [JsonIgnore]

        public int PageNumber
        {
            get { return _pageNumber; }
            set { _pageNumber = value < 1 ? 1 : value; }
        }

        public int PageSize
        {
            get { return _pageSize > _maxPageSize ? _maxPageSize : _pageSize; }
            set { _pageSize = value > _maxPageSize || value < 1 ? _maxPageSize : value; }
        }

        public string SortBy { get; set; }
    }

}
