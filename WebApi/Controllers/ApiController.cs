using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public abstract class ApiController : ControllerBase
    {
        protected readonly ISender Mediator;

        protected ApiController(ISender mediator)
            => Mediator = mediator;
    }
}
