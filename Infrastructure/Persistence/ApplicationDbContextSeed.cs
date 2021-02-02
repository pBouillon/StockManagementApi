using Application.Commons.Interfaces;
using Domain.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Persistence
{
    /// <summary>
    /// Database seeding class exposing several to populate the database on startup
    /// </summary>
    public class ApplicationDbContextSeed
    {
        /// <summary>
        /// Populate the database by creating a sample of some the used entities
        /// </summary>
        /// <param name="context">The context to be used for seeding</param>
        /// <returns>An awaitable task of the operation</returns>
        public static async Task SeedSampleDataAsync(IApplicationDbContext context)
        {
            if (context.Products.Any())
            {
                return;
            }

            await context.Products.AddRangeAsync(
                new Product { Name = "Barley" }, 
                new Product { Name = "Hop" },
                new Product { Name = "Wheat" });

            await context.SaveChangesAsync();
        }
    }
}
