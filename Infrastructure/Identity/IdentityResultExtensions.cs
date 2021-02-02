using System.Linq;
using Application.Commons.Models;
using Application.Commons.Models.Identity;

namespace Infrastructure.Identity
{
    /// <summary>
    /// Extension methods for <see cref="Microsoft.AspNetCore.Identity.IdentityResult"/>
    /// </summary>
    public static class IdentityResultExtensions
    {
        /// <summary>
        /// Convert a raw <see cref="Microsoft.AspNetCore.Identity.IdentityResult"/> result to
        /// <see cref="Result"/>
        /// </summary>
        /// <param name="result">Result to be converted</param>
        /// <returns><inheritdoc cref="Result"/> created from the raw result</returns>
        public static Result ToApplicationResult(this Microsoft.AspNetCore.Identity.IdentityResult result)
            => result.Succeeded
                ? Result.Success()
                : Result.Failure(
                    result.Errors.Select(error => error.Description));

        /// <summary>
        /// Convert a raw <see cref="Microsoft.AspNetCore.Identity.IdentityResult"/> result to
        /// <see cref="Result"/>
        /// </summary>
        /// <param name="result">Result to be converted</param>
        /// <param name="payload">Payload to be wrapped in the result</param>
        /// <typeparam name="TPayload">Type of the payload to be wrapped</typeparam>
        /// <returns><inheritdoc cref="Result"/> created from the raw result</returns>
        public static Result<TPayload> ToApplicationResult<TPayload>(
            this Microsoft.AspNetCore.Identity.IdentityResult result, TPayload payload)
            where TPayload : class
            => result.Succeeded
                ? Result<TPayload>.Success(payload)
                : Result<TPayload>.Failure(
                    result.Errors.Select(error => error.Description));
    }
}
