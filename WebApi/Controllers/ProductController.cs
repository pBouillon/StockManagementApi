using Application.Commons.Dtos;
using Application.Products.Commands.CreateProductCommand;
using Application.Products.Commands.DeleteProductCommand;
using Application.Products.Commands.UpdateProductCommand;
using Application.Products.Queries.GetAllProductsQuery;
using Application.Products.Queries.GetProductQuery;
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
        public async Task<ActionResult<ProductDto>> CreateProductAsync(CreateProductCommand command)
        {
            var createdProduct = await Mediator.Send(command);

            return CreatedAtAction(nameof(GetProductByIdAsync), new { createdProduct.Id }, createdProduct);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProductByIdAsync(int id)
        {
            await Mediator.Send(new DeleteProductCommand { Id = id });

            return NoContent();
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetProductsAsync()
            => Ok(await Mediator.Send(new GetAllProductsQuery()));

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDto>> GetProductByIdAsync(int id)
            => Ok(await Mediator.Send(new GetProductQuery { Id = id }));

        [HttpPut("{id}")]
        public async Task<ActionResult<ProductDto>> UpdateProductById(int id, UpdateProductCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest();
            }

            return Ok(await Mediator.Send(command));
        }
    }
}
