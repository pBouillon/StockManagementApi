using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    public class TestController : ApiController
    {
        public TestController(ISender mediator) 
            : base(mediator) { }

        // TODO: remove (for test purposes only)
        [Authorize(Roles = "Administrator")]
        [HttpGet("admin")]
        public IActionResult AdminExample()
            => Ok();

        // TODO: remove (for test purposes only)
        [Authorize]
        [HttpGet("user")]
        public IActionResult UserExample()
            => Ok();
    }
}
