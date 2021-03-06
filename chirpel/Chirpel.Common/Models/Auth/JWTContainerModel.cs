﻿using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using Chirpel.Contract.Interfaces.Auth;

namespace Chirpel.Contract.Models.Auth
{
    public class JWTContainerModel : IAuthContainerModel
    {
        public string SecurityAlogrithm { get; set; } = SecurityAlgorithms.HmacSha256Signature;
        public int ExpireMinutes { get; set; } = 10080;
        public Claim[] Claims { get; set; }
    }
}
