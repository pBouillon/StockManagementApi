using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Commons.Behaviors
{
    /// <summary>
    /// Pipeline step during which any unhandled exception will be logged
    /// </summary>
    /// <typeparam name="TRequest">Incoming request</typeparam>
    /// <typeparam name="TResponse">Response produced by the request</typeparam>
    public class UnhandledExceptionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        /// <summary>
        /// Request's logger
        /// </summary>
        private readonly ILogger<TRequest> _logger;

        /// <summary>
        /// Create the pipeline step
        /// </summary>
        /// <param name="logger">Request's logger</param>
        public UnhandledExceptionBehavior(ILogger<TRequest> logger)
            => _logger = logger;

        /// <inheritdoc />
        public async Task<TResponse> Handle(
            TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            try
            {
                return await next();
            }
            catch (Exception ex)
            {
                var requestName = typeof(TRequest).Name;

                _logger.LogError(ex, "Unhandled Exception for Request {Name} {@Request}", requestName, request);

                throw;
            }
        }
    }
}
