using Application.Commons.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IApplicationDbContext _context;

        private readonly ILogger<ProductController> _logger;

        public ProductController(IApplicationDbContext context, ILogger<ProductController> logger)
            => (_context, _logger) = (context, logger);

        [HttpDelete("{id}")]
        public IActionResult DeleteById(int id)
        {
            var toRemove = _context.Products.First(product => product.Id == id);
            _context.Products.Remove(toRemove);
            _context.SaveChanges();

            _logger.LogDebug($"Product of id {id} removed");

            return NoContent();
        }

        [HttpGet]
        public ActionResult<IEnumerable<Product>> Get()
            => Ok(_context.Products);

        [HttpGet("{id}")]
        public ActionResult<IEnumerable<Product>> GetById(int id)
            => Ok(_context.Products.First(product => product.Id == id));

        [HttpPost]
        public IActionResult Post()
        {
            var product = _context.Products.Add(new Product
            {
                Name = "Hello World !"
            }).Entity;

            _context.SaveChanges();

            _logger.LogDebug($"Product of id {product.Id} created");

            return CreatedAtAction(nameof(GetById), new { product.Id }, product);
        }
    }
}
