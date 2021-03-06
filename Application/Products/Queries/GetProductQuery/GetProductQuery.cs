﻿using Application.Commons.Exceptions;
using Application.Commons.Interfaces;
using Application.Products.Dtos;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Products.Queries.GetProductQuery
{
    /// <summary>
    /// CQRS query to retrieve a specific <see cref="ProductDto"/>
    /// </summary>
    public class GetProductQuery : IRequest<ProductDto>
    {
        /// <summary>
        /// Id of the product to retrieve
        /// </summary>
        public Guid Id { get; set; }
    }

    /// <summary>
    /// Handler for the <see cref="GetProductQuery"/>
    /// </summary>
    public class GetProductQueryHandler : IRequestHandler<GetProductQuery, ProductDto>
    {
        /// <summary>
        /// Application context
        /// </summary>
        private readonly IApplicationDbContext _context;

        /// <summary>
        /// Operation's logger
        /// </summary>
        private readonly ILogger<GetProductQueryHandler> _logger;

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
        public GetProductQueryHandler(
            IApplicationDbContext context, IMapper mapper, ILogger<GetProductQueryHandler> logger)
            => (_context, _mapper, _logger) = (context, mapper, logger);

        /// <summary>
        /// Retrieve a specific <see cref="Product"/> in the database, based on the incoming
        /// <see cref="GetProductQuery"/>
        /// </summary>
        /// <param name="request">Incoming request to handle</param>
        /// <param name="cancellationToken">
        /// <see cref="CancellationToken"/> used to asynchronously cancel the pending operation
        /// </param>
        /// <returns>The <see cref="Product"/> of the given id, mapped to its <see cref="ProductDto"/></returns>
        public Task<ProductDto> Handle(GetProductQuery request, CancellationToken cancellationToken)
        {
            var entity = _context.Products
                .FirstOrDefault(product => product.Id == request.Id);

            if (entity == null)
            {
                throw new NotFoundException(nameof(Product), new { request.Id });
            }

            return Task.FromResult(_mapper.Map<ProductDto>(entity));
        }
    }
}
