using Application.Authentication.Dtos;
using Application.Commons.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Authentication.Commands
{
    public class LoginCommand : IRequest<AuthenticationResponse>
    {
        public string Username { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;
    }

    public class LoginCommandHandler : IRequestHandler<LoginCommand, AuthenticationResponse>
    {
        private readonly IIdentityService _identityService;

        public LoginCommandHandler(IIdentityService identityService)
            => _identityService = identityService;

        public async Task<AuthenticationResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            // TODO: handle failing auth
            var (token, expireOn) = await _identityService.GetJwtForUserAsync(request.Username, request.Password);
            
            return new AuthenticationResponse
            {
                Token = token,
                ExpireOn = expireOn
            };
        }
    }
}
