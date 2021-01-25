using System.Linq;
using IdentityResult = Application.Commons.Models.IdentityResult;

namespace Infrastructure.Identity
{
    /// <summary>
    /// Extension methods for <see cref="Microsoft.AspNetCore.Identity.IdentityResult"/>
    /// </summary>
    public static class IdentityResultExtensions
    {
        /// <summary>
        /// Convert a raw <see cref="Microsoft.AspNetCore.Identity.IdentityResult"/> result to
        /// <see cref="IdentityResult"/>
        /// </summary>
        /// <param name="result">Result to be converted</param>
        /// <returns><inheritdoc cref="IdentityResult"/> created from the raw result</returns>
        public static IdentityResult ToApplicationResult(this Microsoft.AspNetCore.Identity.IdentityResult result)
        {
            return result.Succeeded
                ? IdentityResult.Success()
                : IdentityResult.Failure(
                    result.Errors.Select(error => error.Description));
        }
    }
}
