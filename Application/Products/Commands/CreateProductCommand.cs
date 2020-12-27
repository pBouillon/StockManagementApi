using Application.Commons.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Products.Commands
{
    public class CreateProductCommand : IRequest<Product>
    {
        public string Name { get; set; } = string.Empty;
    }

    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Product>
    {
        private readonly IApplicationDbContext _context;

        private readonly ILogger<CreateProductCommandHandler> _logger;

        public CreateProductCommandHandler(IApplicationDbContext context, ILogger<CreateProductCommandHandler> logger)
            => (_context, _logger) = (context, logger);

        public async Task<Product> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var entity = new Product
            {
                Name = request.Name,
            };

            await _context.Products.AddAsync(entity, cancellationToken);

            _ = await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation($"{entity} successfully created");

            return entity;
        }
    }
}
