using Application.User.Commands;
using Application.User.Dtos;
using Application.User.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
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
        public async Task<ActionResult<UserDto>> CreateUserAsync(CreateUserCommand command)
        {
            var createdUser = await Mediator.Send(command);

            return CreatedAtAction(nameof(GetUserByIdAsync), new { createdUser.Id }, createdUser);
        }

        /// <summary>
        /// Retrieve a specific user by its id
        /// </summary>
        /// <param name="id">Id of the user to retrieve</param>
        /// <returns>The user matching the provided id</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetUserByIdAsync(Guid id)
            => Ok(await Mediator.Send(new GetUserQuery { Id = id }));
    }
}
