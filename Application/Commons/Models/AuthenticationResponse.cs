using System;

namespace Application.Commons.Models
{
    /// <summary>
    /// User authentication response, holding the user's JWT information
    /// </summary>
    public class AuthenticationResponse
    {
        /// <summary>
        /// <see cref="DateTime"/> on which the forged JWT will expire
        /// </summary>
        public DateTime ExpireOn { get; set; }

        /// <summary>
        /// User's JWT to be attached with his request
        /// </summary>
        public string Token { get; set; } = string.Empty;
    }
}
