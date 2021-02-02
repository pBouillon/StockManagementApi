using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Commons.Mappings
{
    /// <summary>
    /// Represent a paginated collection of items
    /// </summary>
    /// <remarks>
    /// The page index is one-based
    /// </remarks>
    public class PaginatedList<T>
    {
        /// <summary>
        /// Whether or not the page of offset PageIndex + 1 exists and contains any result
        /// </summary>
        public bool HasNextPage
            => PageIndex < TotalPages;

        /// <summary>
        /// Whether or not the page of offset PageIndex - 1 exists and contains any result
        /// </summary>
        public bool HasPreviousPage
            => PageIndex > 1;

        /// <summary>
        /// Collection of held items
        /// </summary>
        public List<T> Items { get; }

        /// <summary>
        /// Index of the current page
        /// </summary>
        public int PageIndex { get; }

        /// <summary>
        /// Number of all items (including the ones in this page)
        /// </summary>
        public int TotalCount { get; }

        /// <summary>
        /// Number of all the pages (including this page)
        /// </summary>
        public int TotalPages { get; }

        /// <summary>
        /// Create the paginated list from a list of item
        /// </summary>
        /// <param name="items">Data source to be paginated</param>
        /// <param name="count">Number of items in the source</param>
        /// <param name="pageIndex">Index of the current page</param>
        /// <param name="pageSize">Maximum number of items per page</param>
        public PaginatedList(List<T> items, int count, int pageIndex, int pageSize)
        {
            PageIndex = pageIndex;
            TotalPages = (int) Math.Ceiling(count / (double) pageSize);
            TotalCount = count;
            Items = items;
        }

        /// <summary>
        /// Create the paginated list from a collection of item
        /// </summary>
        /// <param name="source">Data source to be paginated</param>
        /// <param name="pageIndex">Index of the current page</param>
        /// <param name="pageSize">Maximum number of items per page</param>
        /// <returns></returns>
        public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int pageIndex, int pageSize)
        {
            var count = await source.CountAsync();

            var items = await source.Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PaginatedList<T>(items, count, pageIndex, pageSize);
        }
    }
}
