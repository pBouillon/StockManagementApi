﻿using Application.Commons.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Products.Queries.GetAllProductsQuery
{
    /// <summary>
    /// CQRS query to fetch all the <see cref="Product"/>
    /// </summary>
    public class GetAllProductsQuery : IRequest<IEnumerable<Product>> { }

    /// <summary>
    /// Handler for the <see cref="GetAllProductsQuery"/>
    /// </summary>
    public class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQuery, IEnumerable<Product>>
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
        /// Default constructor for the handler
        /// </summary>
        /// <param name="context">Application context</param>
        /// <param name="logger">Logger</param>
        public GetAllProductsQueryHandler(IApplicationDbContext context, ILogger<GetAllProductsQueryHandler> logger)
            => (_context, _logger) = (context, logger);

        /// <summary>
        /// Retrieve all <see cref="Product"/>
        /// </summary>
        /// <param name="request">Incoming request to handle</param>
        /// <param name="cancellationToken">
        /// <see cref="CancellationToken"/> used to asynchronously cancel the pending operation
        /// </param>
        /// <returns>An array of all the <see cref="Product"/> stored</returns>
        public Task<IEnumerable<Product>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
        {
            var entities = _context.Products.AsEnumerable();

            _logger.LogDebug("All product retrieved");

            return Task.FromResult(entities);
        }
    }
}