﻿using Application.Commons.Exceptions;
using Application.Commons.Interfaces;
using Application.User.Dtos;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.User.Commands.UpdateUserCommand
{
    /// <summary>
    /// CQRS command to update a user
    /// </summary>
    public class UpdateUserCommand : IRequest<UserDto>
    {
        /// <summary>
        /// Id of the user to update
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// New name of the user
        /// </summary>
        public string Username { get; set; } = string.Empty;
    }

    /// <summary>
    /// Handler for the <see cref="UpdateUserCommand"/>
    /// </summary>
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, UserDto>
    {
        /// <summary>
        /// IdentityService instance to manage the authentication logic
        /// </summary>
        private readonly IIdentityService _identityService;
        /// <summary>
        /// Operation's logger
        /// </summary>
        private readonly ILogger<UpdateUserCommandHandler> _logger;

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
        public UpdateUserCommandHandler(
            IIdentityService identityService, ILogger<UpdateUserCommandHandler> logger, IMapper mapper)
            => (_identityService, _logger, _mapper) = (identityService, logger, mapper);

        /// <summary>
        /// Update a user in the database from the incoming <see cref="DeleteUserCommand"/>
        /// </summary>
        /// <param name="request">Incoming request to handle</param>
        /// <param name="cancellationToken">
        /// <see cref="CancellationToken"/> used to asynchronously cancel the pending operation
        /// </param>
        /// <returns>The new state of the resource</returns>
        public async Task<UserDto> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var result = await _identityService.UpdateUsernameAsync(request.Id, request.Username);

            if (!result.Succeeded)
            {
                var identityException = new IdentityException(
                    $"Unable to update the user {request.Username}", result.Errors);

                _logger.LogError(identityException, "Reasons: {}", identityException.Errors);

                throw identityException;
            }

            _logger.LogInformation($"User of id {request.Id} renamed to {request.Username}");

            return _mapper.Map<UserDto>(result.Payload);
        }
    }
}
