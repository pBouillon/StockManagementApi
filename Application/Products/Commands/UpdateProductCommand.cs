using Application.Commons.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Products.Commands
{
    public class UpdateProductCommand : IRequest<Product>
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;
    }

    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, Product>
    {
        private readonly IApplicationDbContext _context;

        private readonly ILogger<UpdateProductCommandHandler> _logger;

        public UpdateProductCommandHandler(IApplicationDbContext context, ILogger<UpdateProductCommandHandler> logger)
            => (_context, _logger) = (context, logger);

        public async Task<Product> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.Products
                .SingleOrDefaultAsync(product => product.Id == request.Id, cancellationToken);

            entity.Name = request.Name;

            _ = _context.SaveChangesAsync(cancellationToken);
            
            _logger.LogInformation($"{entity} renamed to {request.Name}");

            return entity;
        }
    }
}
