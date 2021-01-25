using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Infrastructure.Identity
{
    /// <summary>
    /// TODO
    /// </summary>
    public sealed class IdentityConfiguration
    {
        public int DaysBeforeExpiration { get; set; } = 1;

        public string Secret { get; set; } = string.Empty;

        public string SecurityAlgorithm { get; set; } = SecurityAlgorithms.HmacSha256;

        public string TokenAudience { get; set; } = "http://localhost";

        public string TokenIssuer { get; set; } = "http://localhost";

        public bool TokenRequireHttpsMetadata { get; set; } = true;

        public SymmetricSecurityKey SecurityKey
            => new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Secret));
    }
}
