using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace Chirpel.Contract.Interfaces.Auth
{
    public interface IAuthContainerModel
    {
        string SecurityAlogrithm { get; set; }
        int ExpireMinutes { get; set; }

        Claim[] Claims { get; set; }
    }
}
