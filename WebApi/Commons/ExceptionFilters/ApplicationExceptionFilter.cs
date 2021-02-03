using Application.Commons.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApi.Commons.ExceptionFilters
{
    /// <summary>
    /// Custom exception filter for the exceptions raised by the Application layer
    /// </summary>
    public class ApplicationExceptionFilter : ExceptionFilter<ApplicationException>
    {
        /// <inheritdoc />
        protected override void HandleException(ExceptionContext context, ApplicationException exception)
        {
            context.Result = new BadRequestObjectResult(new
            {
                Error = exception.Message
            });
        }
    }
}
