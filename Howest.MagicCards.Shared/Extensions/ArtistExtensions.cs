using Howest.MagicCards.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Howest.MagicCards.Shared.Extensions
{
    public static class ArtistExtensions
    {
        public static IQueryable<Artist> Sort(this IQueryable<Artist> artists, string sortOrder)
        {
            bool descending = false;

            if (!string.IsNullOrEmpty(sortOrder))
            {
                sortOrder = sortOrder.Trim().ToLower();
                descending = sortOrder.EndsWith("desc");
            }

            return descending
                ? artists.OrderByDescending(a => a.FullName)
                : artists.OrderBy(a => a.FullName);
        }

        public static IQueryable<Artist> Limit(this IQueryable<Artist> artists, int limit)
        {
            if (limit > 0)
            {
                return artists.Take(limit);
            }

            return artists;
        }
    }
}
