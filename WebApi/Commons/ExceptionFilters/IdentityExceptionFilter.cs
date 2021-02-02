using Application.Commons.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApi.Commons.ExceptionFilters
{
    /// <summary>
    /// Custom exception filter for the exceptions raised by IdentityServer
    /// </summary>
    public class IdentityExceptionFilter : ExceptionFilter<IdentityException>
    {
        /// <inheritdoc />
        protected override void HandleException(ExceptionContext context, IdentityException exception)
        {
            context.Result = new BadRequestObjectResult(new
            {
                Error = exception.Message,
                Reasons = exception.Errors
            });
        }
    }
}
