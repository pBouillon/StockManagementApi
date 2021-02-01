using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Identity
{
    /// <summary>
    /// Database seeding class exposing methods to create default roles and users
    /// </summary>
    public static class IdentitySeed
    {
        /// <summary>
        /// Create all the roles by their name as defined in <see cref="ApplicationRoles"/>
        /// </summary>
        /// <remarks>
        /// If any role exists with the same name it will not be created
        /// </remarks>
        /// <param name="roleManager">Identity role manager used to create the roles</param>
        /// <returns>An awaitable task of the operation</returns>
        public static async Task SeedDefaultRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            var rolesToCreate = Enum.GetValues(typeof(ApplicationRoles))
                .Cast<ApplicationRoles>()
                .Select(role => new IdentityRole(role.ToString()))
                .Where(role => roleManager.Roles.All(existingRole => existingRole.Name != role.Name));

            foreach (var role in rolesToCreate)
            {
                await roleManager.CreateAsync(role);
            }
        }

        /// <summary>
        /// Create a default administrator
        /// </summary>
        /// <param name="userManager">Identity user manager used to create the user</param>
        /// <returns>An awaitable task of the operation</returns>
        public static async Task SeedDefaultUsersAsync(UserManager<ApplicationUser> userManager)
        {
            // Create the admin
            var administrator = new ApplicationUser
            {
                UserName = "Administrator"
            };

            await CreateUserWithRolesAsync(administrator, "Administrator1!", userManager, new[]
            {
                ApplicationRoles.Administrator.ToString()
            });

            // Create the user
            var user = new ApplicationUser
            {
                UserName = "User"
            };

            await CreateUserWithRolesAsync(user, "UserPassword1!", userManager, Array.Empty<string>());
        }

        /// <summary>
        /// Create a user with the provided password and assign him the provided role if any
        /// </summary>
        /// <remarks>
        /// If any user exists with the same name, the user will not be created
        /// </remarks>
        /// <param name="user">User to be created</param>
        /// <param name="password">Password to be used for the default user</param>
        /// <param name="userManager"></param>
        /// <param name="roles"></param>
        /// <returns></returns>
        private static async Task CreateUserWithRolesAsync(
            ApplicationUser user, string password, UserManager<ApplicationUser> userManager, IEnumerable<string> roles)
        {
            if (userManager.Users.Any(storedUser => storedUser.UserName != user.UserName))
            {
                return;
            }

            await userManager.CreateAsync(user, password);
            await userManager.AddToRolesAsync(user, roles);
        }

    }
}
