using Application.Commons.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Identity
{
    /// <summary>
    /// TODO
    /// </summary>
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public IdentityService(UserManager<ApplicationUser> userManager)
            => _userManager = userManager;

        public async Task CreateUserAsync(string username, string password)
        {
            var user = new ApplicationUser
            {
                UserName = username,
            };

            await _userManager.CreateAsync(user, password);
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<(string, DateTime)> GetJwtForUserAsync(string username, string password)
        {
            var user = await GetAuthenticatedUserAsync(username, password);

            return await GenerateJwtForUserAsync(user);
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        private async Task<(string, DateTime)> GenerateJwtForUserAsync(ApplicationUser user)
        {
            var userClaims = await GetClaimsAsync(user);

            // TODO to cste
            const string key = "MyApplicationKey";
            const string signingAlgorithm = SecurityAlgorithms.HmacSha256;
            const int daysBeforeExpiration = 5;
            const string issuer = "http://localhost";
            const string audience = "http://localhost";

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                expires: DateTime.Now.AddDays(daysBeforeExpiration),
                claims: userClaims,
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
                    signingAlgorithm)
            );

            return (
                new JwtSecurityTokenHandler().WriteToken(token),
                token.ValidTo
            );
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        private async Task<List<Claim>> GetClaimsAsync(ApplicationUser user)
        {
            var userClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName)
            };

            userClaims.AddRange(await _userManager.GetClaimsAsync(user));

            userClaims.AddRange((await _userManager.GetRolesAsync(user))
                .Select(role => new Claim(ClaimTypes.Role, role)));

            return userClaims;
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        private async Task<ApplicationUser> GetAuthenticatedUserAsync(string username, string password)
        {
            var user = await _userManager.FindByNameAsync(username);

            if (user == null)
            {
                // TODO: custom exception
                throw new Exception("No user found for this name");
            }

            if (!await _userManager.CheckPasswordAsync(user, password))
            {
                // TODO: custom exception
                throw new Exception("Incorrect password for the provided user");
            }

            return user;
        }
    }
}
