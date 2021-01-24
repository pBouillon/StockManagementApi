using System.Threading.Tasks;
using Application.Authentication.Commands;
using Application.Authentication.Dtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    /// <summary>
    /// TODO
    /// </summary>
    public class UserController : ApiController
    {
        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="mediator"></param>
        public UserController(ISender mediator) 
            : base(mediator) { }

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
