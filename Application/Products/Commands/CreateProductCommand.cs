using System.Threading;
using System.Threading.Tasks;
using Application.Commons.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Products.Commands
{
    public class CreateProductCommand : IRequest<Product>
    {
        public string ProductName { get; set; }
    }

    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Product>
    {
        private readonly IApplicationDbContext _context;

        private readonly ILogger<CreateProductCommandHandler> _logger;

        public CreateProductCommandHandler(IApplicationDbContext context, ILogger<CreateProductCommandHandler> logger)
            => (_context, _logger) = (context, logger);


        public async Task<Product> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var product = new Product
            {
                Name = request.ProductName
            };

            await _context.Products.AddAsync(product, cancellationToken);

            _ = await _context.SaveChangesAsync(cancellationToken);

            return product;
        }
    }
}
