using System;

namespace Application.Commons.Models.Identity
{
    /// <summary>
    /// Representation of a user in Identity
    /// </summary>
    public class IdentityUser
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
