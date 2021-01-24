using Application.Authentication.Dtos;
using Application.Commons.Models;
using System.Threading.Tasks;

namespace Application.Commons.Interfaces
{
    /// <summary>
    /// TODO
    /// </summary>
    public interface IIdentityService
    {
        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        Task<IdentityResult> CreateUserAsync(string username, string password);
     
        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        Task<IdentityResult<AuthenticationResponse>> GetJwtForUserAsync(string username, string password);
    }
}
