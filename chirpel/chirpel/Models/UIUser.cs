using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chirpel.Models
{
    public class UIUser
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string Bio { get; set; }
        public string ProfilePicture { get; set; }
        public bool IsPrivate { get; set; }
        public List<string> Followers { get; set; }
        public List<string> Following { get; set; }
        public List<UIPost> Posts { get; set; }   
    }
}
