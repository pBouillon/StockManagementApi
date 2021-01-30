using Application.Commons.Interfaces;
using Application.Commons.Models;
using IdentityModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Application.Commons.Models.Identity;
using IdentityResult = Application.Commons.Models.Identity.IdentityResult;

namespace Infrastructure.Identity
{
    /// <inheritdoc cref="IIdentityService"/>
    public class IdentityService : IIdentityService
    {
        /// <summary>
        /// Configuration class, holding the parameters to initialize IdentityServer's services
        /// </summary>
        private readonly IdentityConfiguration _identityConfiguration;

        /// <summary>
        /// IdentityServer's user manager class
        /// </summary>
        private readonly UserManager<ApplicationUser> _userManager;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="userManager">
        /// Configuration class, holding the parameters to initialize IdentityServer's services
        /// </param>
        /// <param name="identityConfiguration">IdentityServer's user manager class</param>
        public IdentityService(UserManager<ApplicationUser> userManager, IdentityConfiguration identityConfiguration)
            => (_identityConfiguration, _userManager) = (identityConfiguration, userManager);

        /// <inheritdoc />
        public async Task<IdentityResult<CreatedUserResponse>> CreateUserAsync(string username, string password)
        {
            var user = new ApplicationUser
            {
                UserName = username,
            };

            var result = await _userManager.CreateAsync(user, password);

            return result.ToApplicationResult(new CreatedUserResponse
            {
                Id = Guid.Parse(user.Id),
                Username = username
            });
        }
        
        /// <summary>
        /// Generate a JWT for a <see cref="ApplicationUser"/>
        /// </summary>
        /// <param name="user">Registered user for whom the JWT will be created</param>
        /// <returns>The forged JWT and its associated data</returns>
        private async Task<AuthenticationResponse> GenerateJwtForUserAsync(ApplicationUser user)
        {
            var userClaims = await GetClaimsAsync(user);

            var expiresOn = DateTime.Now.AddDays(_identityConfiguration.DaysBeforeExpiration);

            var token = new JwtSecurityToken(
                _identityConfiguration.TokenIssuer,
                _identityConfiguration.TokenAudience,
                expires: expiresOn,
                claims: userClaims,
                signingCredentials: new SigningCredentials(
                    _identityConfiguration.SecurityKey,
                    _identityConfiguration.SecurityAlgorithm)
            );

            return new AuthenticationResponse
            {
                ExpireOn = token.ValidTo,
                Token = new JwtSecurityTokenHandler()
                    .WriteToken(token)
            };
        }

        /// <summary>
        /// Retrieve the registered <see cref="ApplicationUser"/> matching the provided credentials
        /// </summary>
        /// <param name="username">User name</param>
        /// <param name="password">Password of the user, in plain text</param>
        /// <returns>An <see cref="IdentityResult"/> holding the result</returns>
        private async Task<IdentityResult<ApplicationUser>> GetAuthenticatedUserAsync(
            string username, string password)
        {
            var user = await _userManager.FindByNameAsync(username);

            return user != null
                   && await _userManager.CheckPasswordAsync(user, password)
                ? IdentityResult<ApplicationUser>.Success(user)
                : IdentityResult<ApplicationUser>.Failure(new[] { "Invalid credentials" });
        }

        /// <summary>
        /// Get the claims associated to a registered <inheritdoc cref="ApplicationUser"/>
        /// </summary>
        /// <param name="user">Registered user for which the claim will be retrieved</param>
        /// <returns></returns>
        private async Task<List<Claim>> GetClaimsAsync(ApplicationUser user)
        {
            var userClaims = new List<Claim>
            {
                new Claim(JwtClaimTypes.Name, user.UserName),
                new Claim(JwtClaimTypes.Subject, user.Id)
            };

            userClaims.AddRange(await _userManager.GetClaimsAsync(user));

            userClaims.AddRange((await _userManager.GetRolesAsync(user))
                .Select(role => new Claim(JwtClaimTypes.Role, role)));

            return userClaims;
        }
       
        /// <inheritdoc />
        public async Task<IdentityResult<AuthenticationResponse>> GetJwtForUserAsync(string username, string password)
        {
            var userAuthentication = await GetAuthenticatedUserAsync(username, password);

            return !userAuthentication.Succeeded
                ? IdentityResult<AuthenticationResponse>.Failure(
                    userAuthentication.Errors)
                : IdentityResult<AuthenticationResponse>.Success(
                    await GenerateJwtForUserAsync(userAuthentication.Payload!));
        }
    }
}
