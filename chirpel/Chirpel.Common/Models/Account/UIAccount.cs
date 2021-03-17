using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chirpel.Common.Models.Account
{
    public class UIAccount
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string Bio { get; set; }
        public string ProfilePicture { get; set; }
        public List<Guid> Followers { get; set; }
        public List<Guid> Following { get; set; }
        public List<Post> Posts { get; set; }
        public bool IsPrivate { get; set; }
    }
}
