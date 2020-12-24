using Application.Commons.Interfaces;
using Domain.Commons;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Persistence
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        public DbSet<Product> Products { get; set; }

        //public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        //    : base(options) { }

        private readonly IDateTime _dateTime;

        //public ApplicationDbContext(IDateTime dateTime)
        //    => _dateTime = dateTime;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IDateTime dateTime)
            : base(options)
            => _dateTime = dateTime;

        //protected override void OnConfiguring(DbContextOptionsBuilder options)
        //    => options.UseSqlite("Data Source=../stock-management.db");

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.Created = _dateTime.Now;
                        break;

                    case EntityState.Modified:
                        entry.Entity.LastModified = _dateTime.Now;
                        break;
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}