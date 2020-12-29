using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    /// <summary>
    /// Base class for the API controllers.
    /// Produces pluralized routes
    /// </summary>
    [ApiController]
    [Route("api/[controller]s")]
    public abstract class ApiController : ControllerBase
    {
        /// <summary>
        /// Mediator object to send CQRS operations to the Application layer
        /// </summary>
        protected readonly ISender Mediator;

        /// <summary>
        /// Default base constructor
        /// </summary>
        /// <param name="mediator">Mediator object to send CQRS operations to the Application layer</param>
        protected ApiController(ISender mediator)
            => Mediator = mediator;
    }
}
