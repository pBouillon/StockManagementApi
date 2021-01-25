using Application.Authentication.Commands;
using Application.Authentication.Dtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    /// <summary>
    /// REST API controller for the application users
    /// </summary>
    public class UserController : ApiController
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="mediator">Mediator object to send CQRS operations to the Application layer</param>
        public UserController(ISender mediator) 
            : base(mediator) { }

        /// <summary>
        /// Create a new user in the application
        /// </summary>
        /// <param name="command">Payload from which the new user will be created</param>
        /// <returns>A representation of the newly created user</returns>
        [HttpPost]
        public async Task<ActionResult<CreatedUserDto>> CreateUserAsync(CreateUserCommand command)
        {
            var createdUser = await Mediator.Send(command);

            return Ok(createdUser);
            // TODO:
            // return CreatedAtAction(nameof(GetUserByIdAsync), new { createdUser.Id }, createdUser);
        }
    }
}
