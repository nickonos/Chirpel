using Chirpel.Contract.Interfaces.Auth;
using Chirpel.Logic.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Chirpel.Models
{
    public class LoginUser
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
