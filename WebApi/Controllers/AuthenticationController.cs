using Application.Authentication.Commands;
using Application.Commons.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    /// <summary>
    /// REST API controller for user's authentication
    /// </summary>
    public class AuthenticationController : ApiController
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="mediator">Mediator object to send CQRS operations to the Application layer</param>
        public AuthenticationController(ISender mediator)
            : base(mediator) { }

        /// <summary>
        /// Authenticate a user and forge his JWT
        /// </summary>
        /// <param name="command">Payload from which the user will be authenticated</param>
        /// <returns>The user's authentication data</returns>
        [HttpPost]
        public async Task<ActionResult<AuthenticationResponse>> LogUserInAsync(LoginCommand command)
            => Ok(await Mediator.Send(command));
    }
}
