using Application.Commons.Interfaces;
using IdentityModel;
using Microsoft.AspNetCore.Http;
using System;
using System.Security.Claims;

namespace WebApi.Services
{
    /// <inheritdoc cref="ICurrentUserService"/>
    public class CurrentUserService : ICurrentUserService
    {
        /// <summary>
        /// Accessor to the current HTTP context
        /// </summary>
        private readonly IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// Create the current user service
        /// </summary>
        /// <param name="httpContextAccessor">Accessor to the current HTTP context</param>
        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
            => _httpContextAccessor = httpContextAccessor;

        /// <inheritdoc cref="ICurrentUserService"/>
        public Guid UserId
        {
            get
            {
                var rawId = _httpContextAccessor
                    .HttpContext?
                    .User
                    .FindFirstValue(JwtClaimTypes.Id);

                return rawId != null
                    ? Guid.Parse(rawId)
                    : Guid.Empty;
            }
        }
    }
}
