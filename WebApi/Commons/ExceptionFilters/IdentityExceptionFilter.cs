using Application.Commons.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApi.Commons.ExceptionFilters
{
    /// <summary>
    /// TODO
    /// </summary>
    public class IdentityExceptionFilter : ExceptionFilter<IdentityException>
    {
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
