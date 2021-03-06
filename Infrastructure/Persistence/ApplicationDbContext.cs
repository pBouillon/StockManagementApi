using Application.Commons.Interfaces;
using Domain.Commons;
using Domain.Entities;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Persistence
{
    /// <inheritdoc cref="IApplicationDbContext"/>
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>, IApplicationDbContext
    {
        /// <summary>
        /// Injected <see cref="IDateTime"/>
        /// </summary>
        private readonly IDateTime _dateTime;

        /// <summary>
        /// Injected logger
        /// </summary>
        private readonly ILogger<ApplicationDbContext> _logger;

        ///  <inheritdoc cref="IApplicationDbContext.Products"/>
        public DbSet<Product> Products { get; set; } = null!;

        /// <summary>
        /// Default constructor to create the context
        /// </summary>
        /// <param name="options">Database context options to create the context</param>
        /// <param name="dateTime">Injected <see cref="IDateTime"/></param>
        /// <param name="logger">Injected logger</param>
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IDateTime dateTime,
            ILogger<ApplicationDbContext> logger) 
            : base(options)
            => (_dateTime, _logger) = (dateTime, logger);

        /// <summary>
        /// Save the pending changes in the database
        /// <para>
        /// The <see cref="AuditableEntity"/> will be automatically updated based on the type of the operation
        /// performed
        /// </para>
        /// </summary>
        /// <param name="cancellationToken">
        /// <see cref="CancellationToken"/> used to asynchronously cancel the pending operation
        /// </param>
        /// <returns></returns>
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
            {
                var entity = entry.Entity;

                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedOn = _dateTime.Now;
                        _logger.LogInformation("Creation date set for {Entity}", entity);
                        break;

                    case EntityState.Modified:
                        entry.Entity.LastModifiedOn = _dateTime.Now;
                        _logger.LogInformation("Modification date updated for {Entity}", entity);
                        break;
                }
            }

            var saveChangesResult = await base.SaveChangesAsync(cancellationToken);

            return saveChangesResult;
        }
    }
}
