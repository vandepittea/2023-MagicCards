namespace Howest.MagicCards.WebAPI.Extensions
{
    public static class PaginationExtensions
    {
        public static IQueryable<T> ToPagedList<T>(this IQueryable<T> source, int pageNumber, int pageSize, int minPageSize, int maxPageSize)
        {
            pageNumber = Math.Max(1, pageNumber);
            pageSize = Math.Clamp(pageSize, minPageSize, maxPageSize);

            return source.Skip((pageNumber - 1) * pageSize).Take(pageSize);
        }
    }
}