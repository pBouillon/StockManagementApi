using Application.Authentication.Dtos;
using Application.Commons.Models;
using System.Threading.Tasks;

namespace Application.Commons.Interfaces
{
    /// <summary>
    /// Identity service, used to interact with the system authentication mechanisms
    /// </summary>
    public interface IIdentityService
    {
        /// <summary>
        /// Create a new user
        /// </summary>
        /// <param name="username">User name</param>
        /// <param name="password">
        /// User password, according to the policies specified when registering the IdentityServer's services
        /// </param>
        /// <returns>An awaitable task of the create user</returns>
        Task<IdentityResult> CreateUserAsync(string username, string password);

        /// <summary>
        /// Authenticate a user, and get his newly forged JWT
        /// </summary>
        /// <param name="username">User name</param>
        /// <param name="password">The password of the user, in clear text</param>
        /// <returns>An awaitable task of the user's JWT in an <see cref="AuthenticationResponse"/></returns>
        Task<IdentityResult<AuthenticationResponse>> GetJwtForUserAsync(string username, string password);
    }
}
