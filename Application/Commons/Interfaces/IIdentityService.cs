using System;
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
        Task CreateUserAsync(string username, string password);
     
        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        Task<(string, DateTime)> GetJwtForUserAsync(string username, string password);
    }
}
