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
            var logMessage = $"Incoming operation {typeof(TRequest).Name} received";

            if (_currentUserService.IsUserAuthenticated)
            {
                logMessage += $" from the user of id {_currentUserService.UserId}";
            }

            _logger.LogInformation(logMessage);

            return Task.CompletedTask;
        }
    }
}
