using Application.Commons.Exceptions;
using Application.Commons.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.User.Commands.DeleteUserCommand
{
    /// <summary>
    /// CQRS command to delete a user
    /// </summary>
    public class DeleteUserCommand : IRequest
    {
        /// <summary>
        /// Id of the user to delete
        /// </summary>
        public Guid Id { get; set; }
    }

    /// <summary>
    /// Handler for the <see cref="DeleteUserCommand"/>
    /// </summary>
    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand>
    {
        /// <summary>
        /// Accessor for the current user's data
        /// </summary>
        private readonly ICurrentUserService _currentUserService;

        /// <summary>
        /// IdentityService instance to manage the authentication logic
        /// </summary>
        private readonly IIdentityService _identityService;

        /// <summary>
        /// Operation's logger
        /// </summary>
        private readonly ILogger<DeleteUserCommandHandler> _logger;

        /// <summary>
        /// Default constructor for the handler
        /// </summary>
        /// <param name="currentUserService">Accessor for the current user's data</param>
        /// <param name="identityService">IdentityService instance to manage the authentication logic</param>
        /// <param name="logger">Logger</param>
        public DeleteUserCommandHandler(ICurrentUserService currentUserService, IIdentityService identityService,
            ILogger<DeleteUserCommandHandler> logger)
            => (_currentUserService, _identityService, _logger) = (currentUserService, identityService, logger);

        /// <summary>
        /// Delete a user in the database from the incoming <see cref="DeleteUserCommand"/>
        /// </summary>
        /// <param name="request">Incoming request to handle</param>
        /// <param name="cancellationToken">
        /// <see cref="CancellationToken"/> used to asynchronously cancel the pending operation
        /// </param>
        /// <returns>A no-op result</returns>
        public async Task<Unit> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId;

            var isOriginSelfOrAdmin = _currentUserService.UserId == request.Id
                                      || await _identityService.IsAdmin(userId);

            if (!isOriginSelfOrAdmin)
            {
                var unauthorizedException = new UnauthorizedException(userId, $"delete the user of id {request.Id}");
                
                _logger.LogError(
                    unauthorizedException, $"The user of id {userId} attempted to delete the user {request.Id}");

                throw unauthorizedException;
            }

            var result = await _identityService.DeleteUserAsync(request.Id);

            if (!result.Succeeded)
            {
                var unknownUserException = new NotFoundException(nameof(Commons.Models.Identity.User), new { request.Id });

                _logger.LogError(unknownUserException, $"No user found for the provided id {request.Id}");

                throw unknownUserException;
            }

            _logger.LogInformation($"User of id {request.Id} successfully deleted");

            return Unit.Value;
        }
    }
}
