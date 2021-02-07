using Application.Commons.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Commons.Behaviors
{
    /// <summary>
    /// Pipeline step monitoring the time taken to complete the incoming request and logging a warning if it took too
    /// much time
    /// </summary>
    /// <typeparam name="TRequest">Incoming request</typeparam>
    /// <typeparam name="TResponse">Response produced by the request</typeparam>
    public class PerformanceBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        /// <summary>
        /// Number of milliseconds above which the time to execute the request is considered as abnormally long
        /// </summary>
        private const int LongRunningRequestThreshold = 500;

        /// <summary>
        /// Service exposing the current user if any
        /// </summary>
        private readonly ICurrentUserService _currentUserService;

        /// <summary>
        /// Request's logger
        /// </summary>
        private readonly ILogger<TRequest> _logger;

        /// <summary>
        /// Inner timer used to measure the time elapsed to complete an incoming operation
        /// </summary>
        private readonly Stopwatch _timer;

        /// <summary>
        /// Create the pipeline step
        /// </summary>
        /// <param name="currentUserService">Service exposing the current user if any</param>
        /// <param name="logger">Request's logger</param>
        public PerformanceBehavior(ICurrentUserService currentUserService, ILogger<TRequest> logger)
            => (_currentUserService, _logger, _timer) = (currentUserService, logger, new Stopwatch());

        /// <inheritdoc />
        public async Task<TResponse> Handle(
            TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            _timer.Start();

            var response = await next();

            _timer.Stop();

            var elapsedMilliseconds = _timer.ElapsedMilliseconds;

            if (elapsedMilliseconds > LongRunningRequestThreshold)
            {
                var currentUserId = _currentUserService.IsUserAuthenticated
                    ? _currentUserService.UserId.ToString()
                    : "Anonymous";

                _logger.LogWarning("Long running request {@Request} from {@User} ({Milliseconds} milliseconds)",
                    request, currentUserId, elapsedMilliseconds);
            }

            return response;
        }
    }
}
