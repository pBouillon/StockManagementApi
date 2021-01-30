using Application.User.Dtos;
using MediatR;
using System;

namespace Application.User.Queries
{
    /// <summary>
    /// TODO
    /// </summary>
    public class GetUserQuery : IRequest<UserDto>
    {
        /// <summary>
        /// TODO
        /// </summary>
        public Guid Id { get; set; }
    }
}
