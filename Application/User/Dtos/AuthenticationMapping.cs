using Application.Commons.Models.Identity;
using AutoMapper;

namespace Application.User.Dtos
{
    /// <summary>
    /// Mapping profile for the response of the authentication
    /// </summary>
    public class AuthenticationMapping : Profile
    {
        /// <summary>
        /// Create the mappings
        /// </summary>
        public AuthenticationMapping()
        {
            // Map the response of the creation of user to its DTO
            CreateMap<IdentityUser, UserDto>();
        }
    }
}
