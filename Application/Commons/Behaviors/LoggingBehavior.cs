using Application.Commons.Interfaces;
using MediatR.Pipeline;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Commons.Behaviors
{
    /// <summary>
    /// Pipeline step during which the current request along with other information will be logged
    /// </summary>
    /// <remarks>
    /// This will be invoked at runtime
    /// </remarks>
    /// <typeparam name="TRequest">Incoming request</typeparam>
    public class LoggingBehavior<TRequest> : IRequestPreProcessor<TRequest>
        where TRequest : notnull
    {
        /// <summary>
        /// Service exposing the current user if any
        /// </summary>
        private readonly ICurrentUserService _currentUserService;

        /// <summary>
        /// Logger bound to the 
        /// </summary>
        private readonly ILogger<TRequest> _logger;

        /// <summary>
        /// Pipeline step constructor
        /// </summary>
        /// <param name="currentUserService">Service exposing the current user if any</param>
        /// <param name="logger">Pipeline step's logger</param>
        public LoggingBehavior(ICurrentUserService currentUserService, ILogger<TRequest> logger)
            => (_currentUserService, _logger) = (currentUserService, logger);

        /// <inheritdoc />
        public Task Process(TRequest request, CancellationToken cancellationToken)
        {
            var currentUserId = _currentUserService.IsUserAuthenticated
                ? _currentUserService.UserId.ToString()
                : "Anonymous";

            _logger.LogInformation("Incoming operation {@Request} received from {@User}",
                request, currentUserId);

            return Task.CompletedTask;
        }
    }
}
