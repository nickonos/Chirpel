using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace Chirpel.Contract.Interfaces.Auth
{
    public interface IAuthService
    {
        bool IsTokenValid(string token);
        string GenerateToken(IAuthContainerModel model);
        IEnumerable<Claim> GetTokenClaims(string token);
    }
}
