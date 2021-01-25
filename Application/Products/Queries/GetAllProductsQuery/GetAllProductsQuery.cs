using Application.Commons.Interfaces;
using Application.Commons.Mappings;
using Application.Products.Dtos;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Products.Queries.GetAllProductsQuery
{
    /// <summary>
    /// CQRS query to fetch all the <see cref="Product"/>
    /// </summary>
    public class GetAllProductsQuery : IRequest<PaginatedList<ProductDto>>
    {
        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 10;
    }

    /// <summary>
    /// Handler for the <see cref="GetAllProductsQuery"/>
    /// </summary>
    public class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQuery, PaginatedList<ProductDto>>
    {
        /// <summary>
        /// Application context
        /// </summary>
        private readonly IApplicationDbContext _context;

        /// <summary>
        /// Operation's logger
        /// </summary>
        private readonly ILogger<GetAllProductsQueryHandler> _logger;

        /// <summary>
        /// AutoMapper interface to map domain objects to DTO
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>
        /// Default constructor for the handler
        /// </summary>
        /// <param name="context">Application context</param>
        /// <param name="mapper">AutoMapper interface to map domain objects to DTO</param>
        /// <param name="logger">Logger</param>
        public GetAllProductsQueryHandler(
            IApplicationDbContext context, IMapper mapper, ILogger<GetAllProductsQueryHandler> logger)
            => (_context, _mapper, _logger) = (context, mapper, logger);

        /// <summary>
        /// Retrieve all <see cref="Product"/>
        /// </summary>
        /// <param name="request">Incoming request to handle</param>
        /// <param name="cancellationToken">
        /// <see cref="CancellationToken"/> used to asynchronously cancel the pending operation
        /// </param>
        /// <returns>An array of all the <see cref="Product"/> stored</returns>
        public async Task<PaginatedList<ProductDto>> Handle(
            GetAllProductsQuery request, CancellationToken cancellationToken)
        {
            var paginatedEntities = await _context.Products
                .OrderBy(product => product.Id)
                .ProjectTo<ProductDto>(_mapper.ConfigurationProvider)
                .ToPaginatedListAsync(request.PageNumber, request.PageSize);

            _logger.LogInformation(
                $"Retrieved {paginatedEntities.Items.Count} product(s) " +
                $"from the page {paginatedEntities.PageIndex}/{paginatedEntities.TotalPages} " +
                $"({request.PageSize} item(s) displayed per pages)");

            return paginatedEntities;
        }
    }
}
