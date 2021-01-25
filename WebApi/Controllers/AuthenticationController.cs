using Application.Authentication.Commands;
using Application.Authentication.Dtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    /// <summary>
    /// TODO
    /// </summary>
    public class AuthenticationController : ApiController
    {
        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="mediator"></param>
        public AuthenticationController(ISender mediator)
            : base(mediator) { }

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<AuthenticationResponseDto>> LogUserInAsync(LoginCommand command)
            => Ok(await Mediator.Send(command));
    }
}
