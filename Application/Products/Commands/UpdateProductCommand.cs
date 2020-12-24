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
        public int ProductId { get; set; }

        public string ProductName { get; set; }
    }

    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, Product>
    {
        private readonly IApplicationDbContext _context;

        private readonly ILogger<UpdateProductCommandHandler> _logger;

        public UpdateProductCommandHandler(IApplicationDbContext context, ILogger<UpdateProductCommandHandler> logger)
            => (_context, _logger) = (context, logger);

        public async Task<Product> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var toUpdate = await _context.Products
                .SingleOrDefaultAsync(product => product.Id == request.ProductId, cancellationToken);

            toUpdate.Name = request.ProductName;

            _ = _context.SaveChangesAsync(cancellationToken);

            return toUpdate;
        }
    }
}
