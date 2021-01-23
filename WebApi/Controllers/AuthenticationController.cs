using Application.Authentication.Commands;
using Application.Authentication.Dtos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    public class AuthenticationController : ApiController
    {
        public AuthenticationController(ISender mediator)
            : base(mediator) { }

        [HttpPost]
        public async Task<ActionResult<AuthenticationResponse>> CreateJwt(LoginCommand command)
            => Ok(await Mediator.Send(command));

        [HttpPost("/user")]
        public async Task<ActionResult<CreatedUserDto>> CreateUser(CreateUserCommand command)
            => Ok(await Mediator.Send(command));

        [Authorize]
        [HttpGet]
        public IActionResult AuthExample()
            => Ok();
    }
}
