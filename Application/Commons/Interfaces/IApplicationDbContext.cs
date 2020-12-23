using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Application.Commons.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<Product> Products { get; set; }

        int SaveChanges();
    }
}
