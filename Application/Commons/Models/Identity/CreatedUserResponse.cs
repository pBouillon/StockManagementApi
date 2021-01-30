using System;

namespace Application.Commons.Models.Identity
{
    /// <summary>
    /// Identity response returned on the creation of a user
    /// </summary>
    public class CreatedUserResponse
    {
        /// <summary>
        /// Id of the created user
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Name of the created user
        /// </summary>
        public string Username { get; set; } = string.Empty;
    }
}
