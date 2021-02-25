using Application.User.Commands.CreateUserCommand;
using Application.User.Commands.DeleteUserCommand;
using Application.User.Commands.UpdateUserCommand;
using Application.User.Dtos;
using Application.User.Queries.GetUserQuery;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    /// <summary>
    /// REST API controller for the application users
    /// </summary>
    [Authorize]
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
        /// <response code="201">User successfully created</response>
        /// <response code="400">Invalid payload, unable to create the user</response>
        /// <response code="401">Forbidden operation</response>
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public async Task<ActionResult<UserDto>> CreateUserAsync(CreateUserCommand command)
        {
            var createdUser = await Mediator.Send(command);

            return CreatedAtAction(nameof(GetUserByIdAsync), new { createdUser.Id }, createdUser);
        }

        /// <summary>
        /// Delete a user in the application
        /// </summary>
        /// <param name="id">Id of the user to delete</param>
        /// <returns>No content on success</returns>
        /// <response code="204">User successfully deleted</response>
        /// <response code="401">Forbidden operation</response>
        /// <response code="404">Unable to find a user matching the provided id</response>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserByIdAsync(Guid id)
        {
            await Mediator.Send(new DeleteUserCommand { Id = id });

            return NoContent();
        }

        /// <summary>
        /// Retrieve a specific user by its id
        /// </summary>
        /// <param name="id">Id of the user to retrieve</param>
        /// <returns>The user matching the provided id</returns>
        /// <response code="200">User successfully fetched</response>
        /// <response code="401">Forbidden operation</response>
        /// <response code="404">Unable to find a user matching the provided id</response>
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetUserByIdAsync(Guid id)
            => Ok(await Mediator.Send(new GetUserQuery { Id = id }));

        /// <summary>
        /// Update a specific user
        /// </summary>
        /// <param name="id">Id of the user to update</param>
        /// <param name="command">Payload from which the user will be updated</param>
        /// <returns>The updated user's representation</returns>
        /// <response code="200">User successfully updated</response>
        /// <response code="400">Invalid payload, unable to update the user</response>
        /// <response code="401">Forbidden operation</response>
        [HttpPut("{id}")]
        public async Task<ActionResult<UserDto>> UpdateUserById(Guid id, UpdateUserCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest(new { Error = "Ids do not match" });
            }

            return Ok(await Mediator.Send(command));
        }
    }
}
