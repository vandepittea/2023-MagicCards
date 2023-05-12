using Azure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Howest.MagicCards.Shared.Wrappers
{
    public class PagedResponse<T>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public int TotalCount { get; set; }
        public T Data { get; set; }

        public PagedResponse(T data, int pageNumber, int pageSize, int totalCount, int totalPages)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            Data = data;
            TotalCount = totalCount;
            TotalPages = totalPages;
        }
    }
}
