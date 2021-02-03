using Application.Commons.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApi.Commons.ExceptionFilters
{
    /// <summary>
    /// Custom exception filter for the exceptions raised by the Application layer when no entity where found for
    /// a given condition
    /// </summary>
    public class NotFoundExceptionFilter : ExceptionFilter<NotFoundException>
    {
        /// <inheritdoc />
        protected override void HandleException(ExceptionContext context, NotFoundException exception)
        {
            context.Result = new NotFoundObjectResult(new
            {
                Error = exception.Message
            });
        }
    }
}
