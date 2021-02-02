using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Commons.Interfaces
{
    /// <summary>
    /// Application database context, used to interact with the stored entities
    /// </summary>
    public interface IApplicationDbContext
    {
        /// <summary>
        /// Stored <see cref="Product"/>
        /// </summary>
        DbSet<Product> Products { get; set; }

        /// <summary>
        /// Commit the pending changes to the database
        /// </summary>
        /// <param name="cancellationToken">
        /// <see cref="CancellationToken"/> used to asynchronously cancel the pending operation
        /// </param>
        /// <returns>A task wrapping the result of the base operation</returns>
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
