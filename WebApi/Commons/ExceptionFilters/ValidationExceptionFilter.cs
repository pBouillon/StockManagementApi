using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;

namespace WebApi.Commons.ExceptionFilters
{
    /// <summary>
    /// Custom exception filter for the exceptions raised by FluentValidator
    /// </summary>
    public class ValidationExceptionFilter : IExceptionFilter
    {
        /// <inheritdoc />
        public void OnException(ExceptionContext context)
        {
            // Check if the exception is the one we would like to handle
            if (!(context.Exception is ValidationException validationException))
            {
                return;
            }

            // Digest the FluentValidation exception errors as a dictionary of:
            // [Property] : Error message
            var errorMessages = validationException.Errors
                .ToDictionary(
                    error => error.PropertyName,
                    error => error.ErrorMessage);

            context.Result = new BadRequestObjectResult(errorMessages);

            context.ExceptionHandled = true;
        }
    }
}
