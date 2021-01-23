using Application.Commons.Models;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using IdentityResult = Application.Commons.Models.IdentityResult;

namespace Infrastructure.Identity
{
    public static class IdentityResultExtensions
    {
        public static IdentityResult ToApplicationResult(this Microsoft.AspNetCore.Identity.IdentityResult result)
        {
            return result.Succeeded
                ? IdentityResult.Success()
                : IdentityResult.Failure(result.Errors.Select(e => e.Description));
        }
    }
}
