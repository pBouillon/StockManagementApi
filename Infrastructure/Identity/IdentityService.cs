﻿using Application.Authentication.Dtos;
using Application.Commons.Interfaces;
using Application.Commons.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using IdentityResult = Application.Commons.Models.IdentityResult;

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

        public async Task<IdentityResult> CreateUserAsync(string username, string password)
        {
            var user = new ApplicationUser
            {
                UserName = username,
            };

            var result = await _userManager.CreateAsync(user, password);

            return result.ToApplicationResult();
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<IdentityResult<AuthenticationResponse>> GetJwtForUserAsync(string username, string password)
        {
            var userAuthentication = await GetAuthenticatedUserAsync(username, password);

            return !userAuthentication.Succeeded 
                ? IdentityResult<AuthenticationResponse>.Failure(
                    userAuthentication.Errors)
                : IdentityResult<AuthenticationResponse>.Success(
                    await GenerateJwtForUserAsync(userAuthentication.Payload));
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        private async Task<AuthenticationResponse> GenerateJwtForUserAsync(ApplicationUser user)
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

            return new AuthenticationResponse
            {
                ExpireOn = token.ValidTo,
                Token = new JwtSecurityTokenHandler().WriteToken(token)
            };
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
        private async Task<IdentityResult<ApplicationUser>> GetAuthenticatedUserAsync(
            string username, string password)
        {
            var user = await _userManager.FindByNameAsync(username);

            // TODO: account lock or delay

            if (user == null
                || !await _userManager.CheckPasswordAsync(user, password))
            {
                return IdentityResult<ApplicationUser>.Failure(new[] {"Invalid credentials"});
            }

            return IdentityResult<ApplicationUser>.Success(user);
        }
    }
}
