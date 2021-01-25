using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Infrastructure.Identity
{
    /// <summary>
    /// Configuration class, holding the parameters to initialize IdentityServer's services
    /// </summary>
    public sealed class IdentityConfiguration
    {
        /// <summary>
        /// Days before a forged token expires
        /// </summary>
        public int DaysBeforeExpiration { get; set; } = 1;

        /// <summary>
        /// Application's secret, used to encode the JWT
        /// </summary>
        public string Secret { get; set; } = string.Empty;

        /// <summary>
        /// Algorithm used to encode the JWT
        /// </summary>
        public string SecurityAlgorithm { get; set; } = SecurityAlgorithms.HmacSha256;

        /// <summary>
        /// Audience for which the JWT is forged
        /// </summary>
        public string TokenAudience { get; set; } = "http://localhost";

        /// <summary>
        /// Issuer of the JWT
        /// </summary>
        public string TokenIssuer { get; set; } = "http://localhost";

        /// <summary>
        /// Whether or not the token audience should be using HTTPS
        /// </summary>
        public bool TokenRequireHttpsMetadata { get; set; } = true;

        /// <summary>
        /// Retrieve the <see cref="SymmetricSecurityKey"/> to be used to forge the JWT
        /// </summary>
        public SymmetricSecurityKey SecurityKey
            => new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Secret));
    }
}
