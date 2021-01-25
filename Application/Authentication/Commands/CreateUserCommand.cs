using Application.Authentication.Dtos;
using Application.Commons.Exceptions;
using Application.Commons.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Authentication.Commands
{
    /// <summary>
    /// CQRS command to create a new user in the system
    /// </summary>
    public class CreateUserCommand : IRequest<CreatedUserDto>
    {
        /// <summary>
        /// Name of the user to be created
        /// </summary>
        public string Username { get; set; } = string.Empty;

        /// <summary>
        /// Password of the user, according to the IdentityServer settings defined when
        /// the service was registered
        /// </summary>
        public string Password { get; set; } = string.Empty;
    }

    /// <summary>
    /// Handler for the <see cref="CreateUserCommand"/>
    /// </summary>
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, CreatedUserDto>
    {
        /// <summary>
        /// IdentityService instance to manage the authentication logic
        /// </summary>
        private readonly IIdentityService _identityService;

        /// <summary>
        /// Operation's logger
        /// </summary>
        private readonly ILogger<CreateUserCommandHandler> _logger;

        /// <summary>
        /// Default constructor for the handler
        /// </summary>
        /// <param name="identityService">IdentityService instance to manage the authentication logic</param>
        /// <param name="logger">Operation's logger</param>
        public CreateUserCommandHandler(IIdentityService identityService, ILogger<CreateUserCommandHandler> logger)
            => (_identityService, _logger) = (identityService, logger);

        /// <summary>
        /// Create a new user in the system
        /// </summary>
        /// <param name="request">Incoming request to handle</param>
        /// <param name="cancellationToken">
        /// <see cref="CancellationToken"/> used to asynchronously cancel the pending operation
        /// </param>
        /// <returns>The <see cref="CreatedUserDto"/> associated to the created user</returns>
        public async Task<CreatedUserDto> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var result = await _identityService.CreateUserAsync(request.Username, request.Password);

            if (!result.Succeeded)
            {
                var identityException = new IdentityException(
                    $"Unable to create the user {request.Username}", result.Errors);

                _logger.LogError(identityException.Message, identityException);

                throw identityException;
            }

            _logger.LogInformation($"User {request.Username} successfully created");

            return new CreatedUserDto
            {
                Username = request.Username
            };
        }
    }
}
