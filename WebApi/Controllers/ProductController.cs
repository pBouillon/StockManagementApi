using Application.Commons.Mappings;
using Application.Products.Commands.CreateProductCommand;
using Application.Products.Commands.DeleteProductCommand;
using Application.Products.Commands.UpdateProductCommand;
using Application.Products.Dtos;
using Application.Products.Queries.GetAllProductsQuery;
using Application.Products.Queries.GetProductQuery;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    /// <summary>
    /// REST API controller for the product resources
    /// </summary>
    [Authorize]
    public class ProductController : ApiController
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="mediator">Mediator object to send CQRS operations to the Application layer</param>
        public ProductController(ISender mediator)
            : base(mediator) { }

        /// <summary>
        /// Create a new product from the payload
        /// </summary>
        /// <param name="command">Payload from which the product will be created</param>
        /// <returns>The newly created product's representation</returns>
        /// <response code="201">Product successfully created</response>
        /// <response code="400">Invalid payload, unable to create the product</response>
        /// <response code="401">Forbidden operation for a non-authenticated user</response>
        [HttpPost]
        public async Task<ActionResult<ProductDto>> CreateProductAsync(CreateProductCommand command)
        {
            var createdProduct = await Mediator.Send(command);

            return CreatedAtAction(nameof(GetProductByIdAsync), new { createdProduct.Id }, createdProduct);
        }

        /// <summary>
        /// Delete a product by its id
        /// </summary>
        /// <param name="id">Id of the item to be deleted</param>
        /// <returns>No content on success</returns>
        /// <response code="204">Product successfully deleted</response>
        /// <response code="401">Forbidden operation for a non-authenticated user</response>
        /// <response code="404">Unable to find a product matching the provided id</response>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProductByIdAsync(Guid id)
        {
            await Mediator.Send(new DeleteProductCommand { Id = id });

            return NoContent();
        }

        /// <summary>
        /// Retrieve all products
        /// </summary>
        /// <param name="query">Additional parameters to narrow down the search</param>
        /// <returns>A paginated list of all the results retrieved</returns>
        /// <response code="200">Products successfully fetched</response>
        /// <response code="401">Forbidden operation for a non-authenticated user</response>
        [HttpGet]
        public async Task<ActionResult<PaginatedList<ProductDto>>> GetProductsAsync(
            [FromQuery] GetAllProductsQuery query)
            => Ok(await Mediator.Send(query));

        /// <summary>
        /// Retrieve a specific product
        /// </summary>
        /// <param name="id">Id of the product to retrieve</param>
        /// <returns>The product matching the provided id</returns>
        /// <response code="204">Product successfully fetched</response>
        /// <response code="401">Forbidden operation for a non-authenticated user</response>
        /// <response code="404">Unable to find a product matching the provided id</response>
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDto>> GetProductByIdAsync(Guid id)
            => Ok(await Mediator.Send(new GetProductQuery { Id = id }));

        /// <summary>
        /// Update a specific product
        /// </summary>
        /// <param name="id">Id of the product to update</param>
        /// <param name="command">Payload from which the product will be updated</param>
        /// <returns>The updated product's representation</returns>
        /// <response code="200">Product successfully updated</response>
        /// <response code="400">Invalid payload</response>
        /// <response code="401">Forbidden operation for a non-authenticated user</response>
        /// <response code="404">Unable to find a product matching the provided id</response>
        [HttpPut("{id}")]
        public async Task<ActionResult<ProductDto>> UpdateProductById(Guid id, UpdateProductCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest(new { Error = "Ids do not match" });
            }

            return Ok(await Mediator.Send(command));
        }
    }
}
