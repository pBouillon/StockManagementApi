using System;
using System.Threading.Tasks;
using Application.Commons.Models;

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
        Task<(string, DateTime)> GetJwtForUserAsync(string username, string password);
    }
}
