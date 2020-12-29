using System.Linq;
using System.Threading.Tasks;

namespace Application.Commons.Mappings
{
    /// <summary>
    /// Custom mapping extension to ease projections
    /// </summary>
    public static class MappingExtensions
    {
        /// <summary>
        /// Project a collection of data to a <see cref="PaginatedList{T}"/>
        /// </summary>
        /// <typeparam name="TDestination">Type of the items wrapped in the <see cref="PaginatedList{T}"/></typeparam>
        /// <param name="queryable">Collection to be mapped</param>
        /// <param name="pageNumber">Page offset of the page (1-based)</param>
        /// <param name="pageSize">Maximum number of items per pages</param>
        /// <returns>The paginated collection</returns>
        public static Task<PaginatedList<TDestination>> ToPaginatedListAsync<TDestination>(
            this IQueryable<TDestination> queryable, int pageNumber, int pageSize)
            => PaginatedList<TDestination>.CreateAsync(queryable, pageNumber, pageSize);
    }
}
