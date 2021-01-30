using Application.Commons.Interfaces;
using Application.User.Dtos;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Commons.Exceptions;
using Application.Commons.Models.Identity;

namespace Application.User.Queries.GetUserQuery
{
    /// <summary>
    /// CQRS query to retrieve a specific <see cref="UserDto"/>
    /// </summary>
    public class GetUserQuery : IRequest<UserDto>
    {
        /// <summary>
        /// Id of the user to retrieve
        /// </summary>
        public Guid Id { get; set; }
    }

    /// <summary>
    /// Handler for the <see cref="GetUserQuery"/>
    /// </summary>
    public class GetUserQueryHandler : IRequestHandler<GetUserQuery, UserDto>
    {
        /// <summary>
        /// IdentityService instance to manage the authentication logic
        /// </summary>
        private readonly IIdentityService _identityService;

        /// <summary>
        /// Operation's logger
        /// </summary>
        private readonly ILogger<GetUserQueryHandler> _logger;

        /// <summary>
        /// AutoMapper interface to map domain objects to DTO
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>
        /// Default constructor for the handler
        /// </summary>
        /// <param name="identityService">IdentityService instance to manage the authentication logic</param>
        /// <param name="logger">Operation's logger</param>
        /// <param name="mapper">AutoMapper interface to map domain objects to DTO</param>
        public GetUserQueryHandler(
            IIdentityService identityService, ILogger<GetUserQueryHandler> logger, IMapper mapper)
            => (_identityService, _logger, _mapper) = (identityService, logger, mapper);

        /// <summary>
        /// Get a user according to its ID
        /// </summary>
        /// <param name="request">Incoming request to handle</param>
        /// <param name="cancellationToken">
        /// <see cref="CancellationToken"/> used to asynchronously cancel the pending operation
        /// </param>
        /// <returns>The <see cref="UserDto"/> associated to the created user</returns>
        public async Task<UserDto> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            var result = await _identityService.GetUserAsync(request.Id);

            if (!result.Succeeded)
            {
                var unknownUserException = new NotFoundException(nameof(Commons.Models.Identity.User), new { request.Id });

                _logger.LogError(unknownUserException, $"No user found for the provided id {request.Id}");

                throw unknownUserException;
            }

            _logger.LogDebug($"User of id { request.Id } retrieved { result }");

            return _mapper.Map<UserDto>(result.Payload);
        }
    }
}
