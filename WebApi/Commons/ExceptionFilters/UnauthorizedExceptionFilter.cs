using Application.Commons.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApi.Commons.ExceptionFilters
{
    /// <summary>
    /// Custom exception filter for the exceptions raised by the Application layer when a user attempted to perform an
    /// operation he is not allowed to
    /// </summary>
    public class UnauthorizedExceptionFilter : ExceptionFilter<UnauthorizedException>
    {
        /// <inheritdoc />
        protected override void HandleException(ExceptionContext context, UnauthorizedException exception)
        {
            context.Result = new UnauthorizedObjectResult(new
            {
                Error = exception.Message
            });
        }
    }
}
