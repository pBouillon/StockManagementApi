using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApi.Commons.ExceptionFilters
{
    /// <summary>
    /// Exception filter targeting a specific exception type
    /// </summary>
    /// <typeparam name="TException">Type of the exception handled by this filter</typeparam>
    public abstract class ExceptionFilter<TException> : IExceptionFilter
    {
        /// <summary>
        /// Process the received exception
        /// </summary>
        /// <param name="context">Exception context</param>
        /// <param name="exception">Typed intercepted exception</param>
        protected abstract void HandleException(ExceptionContext context, TException exception);

        /// <inheritdoc />
        public void OnException(ExceptionContext context)
        {
            // Check if the exception is the one we would like to handle
            if (!(context.Exception is TException exception))
            {
                return;
            }

            HandleException(context, exception);

            context.ExceptionHandled = true;
        }
    }
}
