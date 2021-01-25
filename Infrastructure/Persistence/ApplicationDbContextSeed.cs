using Application.Commons.Interfaces;
using Domain.Entities;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Persistence
{
    /// <summary>
    /// TODO
    /// </summary>
    public class ApplicationDbContextSeed
    {
        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="userManager"></param>
        /// <param name="roleManager"></param>
        /// <returns></returns>
        public static async Task SeedDefaultUserAsync(
            UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            var administratorRole = new IdentityRole("Administrator");

            if (roleManager.Roles.All(role => role.Name != administratorRole.Name))
            {
                await roleManager.CreateAsync(administratorRole);
            }

            var administrator = new ApplicationUser
            {
                UserName = "Administrator",
            };

            if (userManager.Users.All(user => user.UserName != administrator.UserName))
            {
                await userManager.CreateAsync(administrator, "Administrator1!");
                await userManager.AddToRolesAsync(administrator, new[] { administratorRole.Name });
            }
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static async Task SeedSampleDataAsync(IApplicationDbContext context)
        {
            if (context.Products.Any())
            {
                return;
            }

            await context.Products.AddAsync(new Product
            {
                Name = "Hop"
            });

            await context.SaveChangesAsync();
        }
    }
}
