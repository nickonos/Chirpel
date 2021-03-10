using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using Chirpel.Common.Models.Auth;

namespace Chirpel.Logic.Auth
{
    public interface IAuthService
    {
        string SecretKey { get; set; }

        bool IsTokenValid(string token);
        string GenerateToken(IAuthContainerModel model);
        IEnumerable<Claim> GetTokenClaims(string token);
    }
}
