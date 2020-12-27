using Application.Commons.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Products.Commands
{
    public class DeleteProductCommand : IRequest
    {
        public int Id { get; set; }
    }

    public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand>
    {
        private readonly IApplicationDbContext _context;

        private readonly ILogger<DeleteProductCommandHandler> _logger;

        public DeleteProductCommandHandler(IApplicationDbContext context, ILogger<DeleteProductCommandHandler> logger)
            => (_context, _logger) = (context, logger);

        public async Task<Unit> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.Products
                .SingleOrDefaultAsync(product => product.Id == request.Id, cancellationToken);

            _context.Products.Remove(entity);

            _ = await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation($"Product {entity} successfully deleted");

            return Unit.Value;
        }
    }
}
