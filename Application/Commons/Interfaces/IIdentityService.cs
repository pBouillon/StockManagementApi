using Application.Commons.Models.Identity;
using System;
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
        Task<Result<Models.Identity.User>> CreateUserAsync(string username, string password);

        /// <summary>
        /// Delete the user associated to the Id
        /// </summary>
        /// <param name="id">Id associated to the user to delete</param>
        /// <returns>An awaitable task of the <see cref="Result"/> of the operation</returns>
        Task<Result> DeleteUserAsync(Guid id);

        /// <summary>
        /// Authenticate a user, and get his newly forged JWT
        /// </summary>
        /// <param name="username">User name</param>
        /// <param name="password">The password of the user, in clear text</param>
        /// <returns>An awaitable task of the user's JWT in an <see cref="AuthenticationResponse"/></returns>
        Task<Result<AuthenticationResponse>> GetJwtForUserAsync(string username, string password);

        /// <summary>
        /// Retrieve a user by its Id
        /// </summary>
        /// <param name="id">Id of the user to retrieve</param>
        /// <returns>An awaitable task of the user's data</returns>
        Task<Result<Models.Identity.User>> GetUserAsync(Guid id);

        /// <summary>
        /// Update the username of a user
        /// </summary>
        /// <param name="id">Id of the user to update</param>
        /// <param name="username">New name of the user</param>
        /// <returns>An awaitable task of the <see cref="Result"/> of the operation</returns>
        Task<Result<Models.Identity.User>> UpdateUsernameAsync(Guid id, string username);
    }
}
