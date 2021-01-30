using System.Linq;
using Application.Commons.Models;
using Application.Commons.Models.Identity;
using IdentityResult = Application.Commons.Models.Identity.IdentityResult;

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
            => result.Succeeded
                ? IdentityResult.Success()
                : IdentityResult.Failure(
                    result.Errors.Select(error => error.Description));

        /// <summary>
        /// Convert a raw <see cref="Microsoft.AspNetCore.Identity.IdentityResult"/> result to
        /// <see cref="IdentityResult{T}"/>
        /// </summary>
        /// <param name="result">Result to be converted</param>
        /// <param name="payload">Payload to be wrapped in the result</param>
        /// <typeparam name="TPayload">Type of the payload to be wrapped</typeparam>
        /// <returns><inheritdoc cref="IdentityResult{T}"/> created from the raw result</returns>
        public static IdentityResult<TPayload> ToApplicationResult<TPayload>(
            this Microsoft.AspNetCore.Identity.IdentityResult result, TPayload payload)
            where TPayload : class
            => result.Succeeded
                ? IdentityResult<TPayload>.Success(payload)
                : IdentityResult<TPayload>.Failure(
                    result.Errors.Select(error => error.Description));
    }
}
