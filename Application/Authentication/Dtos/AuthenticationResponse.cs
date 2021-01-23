using System;

namespace Application.Authentication.Dtos
{
    public class AuthenticationResponse
    {
        public DateTime ExpireOn { get; set; }

        public string Token { get; set; } = string.Empty;
    }
}
