using Application.Products.Commands;
using Application.Products.Queries;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    public class ProductController : ApiController
    {
        public ProductController(ISender mediator)
            : base(mediator) { }

        [HttpPost]
        public async Task<IActionResult> CreateProductAsync([FromBody] CreateProductCommand command)
        {
            var createdProduct = await Mediator.Send(command);

            return CreatedAtAction(nameof(GetProductByIdAsync), new { createdProduct.Id }, createdProduct);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProductByIdAsync(int id)
        {
            await Mediator.Send(new DeleteProductCommand { ProductId = id });

            return NoContent();
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductsAsync()
            => Ok(await Mediator.Send(new GetAllProductsQuery()));

        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductByIdAsync(int id)
            => Ok(await Mediator.Send(new GetProductQuery { ProductId = id }));
    }
}
