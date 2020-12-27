using Application.Commons.Interfaces;
using Domain.Commons;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Persistence
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        public DbSet<Product> Products { get; set; }

        private readonly IDateTime _dateTime;

        private readonly ILogger<ApplicationDbContext> _logger;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IDateTime dateTime,
            ILogger<ApplicationDbContext> logger) 
            : base(options)
            => (_dateTime, _logger) = (dateTime, logger);

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedOn = _dateTime.Now;
                        _logger.LogInformation("Creation date set");
                        break;

                    case EntityState.Modified:
                        entry.Entity.LastModifiedOn = _dateTime.Now;
                        _logger.LogInformation($"Modification date updated for {entry}");
                        break;
                }
            }

            var saveChangesResult = await base.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Changes committed to the database");

            return saveChangesResult;
        }
    }
}
