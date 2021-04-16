using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using Chirpel.Common.Interfaces.Auth;

namespace Chirpel.Common.Models.Auth
{
    public class JWTContainerModel : IAuthContainerModel
    {
        public string SecretKey { get; set; } = Environment.GetEnvironmentVariable("CHIRPEL_SECRET") ?? "YWJjZGVmZ2hpamtsbW5vcHE=";
        public string SecurityAlogrithm { get; set; } = SecurityAlgorithms.HmacSha256Signature;
        public int ExpireMinutes { get; set; } = 10080;
        public Claim[] Claims { get; set; }
    }
}
