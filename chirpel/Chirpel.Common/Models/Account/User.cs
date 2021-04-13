using System;
using System.Collections.Generic;
using System.Text;

namespace Chirpel.Common.Models.Account
{
    public class User
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
