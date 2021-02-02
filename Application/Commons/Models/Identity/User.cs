using System;

namespace Application.Commons.Models.Identity
{
    /// <summary>
    /// Representation of a user in Identity
    /// </summary>
    public class User
    {
        /// <summary>
        /// Maximum length of the user's name
        /// </summary>
        public const int UsernameMaximumLength = 32;

        /// <summary>
        /// Minimum length of the user's name
        /// </summary>
        public const int UsernameMinimumLength = 4;

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
