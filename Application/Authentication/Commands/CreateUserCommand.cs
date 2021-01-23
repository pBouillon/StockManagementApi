using Application.Authentication.Dtos;
using Application.Commons.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Application.Commons.Exceptions;

namespace Application.Authentication.Commands
{
    public class CreateUserCommand : IRequest<CreatedUserDto>
    {
        public string Username { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;
    }

    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, CreatedUserDto>
    {
        private readonly IIdentityService _identityService;

        public CreateUserCommandHandler(IIdentityService identityService)
            => _identityService = identityService;

        public async Task<CreatedUserDto> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            // TODO: logging
            var result = await _identityService.CreateUserAsync(request.Username, request.Password);

            if (!result.Succeeded)
            {
                throw new IdentityException($"Unable to create the user {request.Username}", result);
            }

            return new CreatedUserDto
            {
                Username = request.Username
            };
        }
    }
}
