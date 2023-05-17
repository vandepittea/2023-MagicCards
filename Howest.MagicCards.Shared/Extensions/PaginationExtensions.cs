namespace Howest.MagicCards.Shared.Extensions
{
    public static class PaginationExtensions
    {
        public static IQueryable<T> ToPagedList<T>(this IQueryable<T> source, int pageNumber, int pageSize, int maxPageSize)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (pageNumber < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(pageNumber), "Page number cannot be less than 1.");
            }

            if (pageSize < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(pageSize), "Page size cannot be less than 1.");
            }
            if (pageSize > maxPageSize)
            {
                throw new ArgumentOutOfRangeException(nameof(pageSize), "Page size cannot be more than " + maxPageSize + ".");
            }

            return source.Skip((pageNumber - 1) * pageSize).Take(pageSize);
        }
    }
}
