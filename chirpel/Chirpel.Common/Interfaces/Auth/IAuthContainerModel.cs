using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace Chirpel.Common.Interfaces.Auth
{
    public interface IAuthContainerModel
    {
        string SecretKey { get; set; }
        string SecurityAlogrithm { get; set; }
        int ExpireMinutes { get; set; }

        Claim[] Claims { get; set; }
    }
}
