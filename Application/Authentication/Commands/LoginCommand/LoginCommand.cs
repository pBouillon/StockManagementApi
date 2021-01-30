using Application.Commons.Exceptions;
using Application.Commons.Interfaces;
using Application.Commons.Models.Identity;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Authentication.Commands.LoginCommand
{
    /// <summary>
    /// CQRS command to log a user in and forge his JWT
    /// </summary>
    public class LoginCommand : IRequest<AuthenticationResponse>
    {
        /// <summary>
        /// Name of the user
        /// </summary>
        public string Username { get; set; } = string.Empty;

        /// <summary>
        /// Password, in clear text, to authenticate the user
        /// </summary>
        public string Password { get; set; } = string.Empty;
    }

    /// <summary>
    /// Handler for the <see cref="LoginCommand"/>
    /// </summary>
    public class LoginCommandHandler : IRequestHandler<LoginCommand, AuthenticationResponse>
    {
        /// <summary>
        /// IdentityService instance to manage the authentication logic
        /// </summary>
        private readonly IIdentityService _identityService;

        /// <summary>
        /// Operation's logger
        /// </summary>
        private readonly ILogger<LoginCommandHandler> _logger;

        /// <summary>
        /// Default constructor for the handler
        /// </summary>
        /// <param name="identityService">IdentityService instance to manage the authentication logic</param>
        /// <param name="logger">Operation's logger</param>
        public LoginCommandHandler(IIdentityService identityService, ILogger<LoginCommandHandler> logger)
            => (_identityService, _logger) = (identityService, logger);

        /// <summary>
        /// Log a user in and forge his JWT
        /// </summary>
        /// <param name="request">Incoming request to handle</param>
        /// <param name="cancellationToken">
        /// <see cref="CancellationToken"/> used to asynchronously cancel the pending operation
        /// </param>
        /// <returns>The <see cref="AuthenticationResponse"/> holding the user's JWT</returns>
        public async Task<AuthenticationResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var result = await _identityService.GetJwtForUserAsync(request.Username, request.Password);

            if (!result.Succeeded)
            {
                var identityException = new IdentityException(
                    $"Unable to forge a JWT for the user '{ request.Username }'", result.Errors);

                _logger.LogError(identityException.Message);

                throw identityException;
            }

            _logger.LogInformation($"User {request.Username} successfully logged in");
            
            return result.Payload;
        }
    }
}
