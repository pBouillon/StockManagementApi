using Application.Commons.Exceptions;
using Application.Commons.Interfaces;
using Application.User.Dtos;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace Application.User.Commands
{
    /// <summary>
    /// CQRS command to create a new user in the system
    /// </summary>
    public class CreateUserCommand : IRequest<UserDto>
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
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, UserDto>
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
        /// AutoMapper interface to map domain objects to DTO
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>
        /// Default constructor for the handler
        /// </summary>
        /// <param name="identityService">IdentityService instance to manage the authentication logic</param>
        /// <param name="logger">Operation's logger</param>
        /// <param name="mapper">AutoMapper interface to map domain objects to DTO</param>
        public CreateUserCommandHandler(
            IIdentityService identityService, ILogger<CreateUserCommandHandler> logger, IMapper mapper)
            => (_identityService, _logger, _mapper) = (identityService, logger, mapper);

        /// <summary>
        /// Create a new user in the system
        /// </summary>
        /// <param name="request">Incoming request to handle</param>
        /// <param name="cancellationToken">
        /// <see cref="CancellationToken"/> used to asynchronously cancel the pending operation
        /// </param>
        /// <returns>The <see cref="UserDto"/> associated to the created user</returns>
        public async Task<UserDto> Handle(CreateUserCommand request, CancellationToken cancellationToken)
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

            return _mapper.Map<UserDto>(result.Payload);
        }
    }
}
