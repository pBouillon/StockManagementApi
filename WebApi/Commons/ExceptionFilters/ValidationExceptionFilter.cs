using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;

namespace WebApi.Commons.ExceptionFilters
{
    /// <summary>
    /// Custom exception filter for the exceptions raised by FluentValidator
    /// </summary>
    public class ValidationExceptionFilter : ExceptionFilter<ValidationException>
    {
        /// <inheritdoc />
        protected override void HandleException(ExceptionContext context, ValidationException exception)
        {
            // Digest the FluentValidation exception errors as a dictionary of: "[Property] : Error message"
            var errorMessages = exception.Errors
                .OrderBy(error => error.PropertyName)
                .ToDictionary(
                    error => error.PropertyName,
                    error => error.ErrorMessage);

            context.Result = new BadRequestObjectResult(errorMessages);
        }
    }
}
