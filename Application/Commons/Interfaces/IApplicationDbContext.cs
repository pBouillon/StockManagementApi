using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using Domain.Entities;

namespace Application.Commons.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<Product> Products { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
