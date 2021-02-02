using System;

namespace Application.User.Dtos
{
    /// <summary>
    /// DTO representing a user
    /// </summary>
    public class UserDto
    {
        /// <summary>
        /// Id of the user
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Name of the user
        /// </summary>
        public string Username { get; set; } = string.Empty;
    }
}
