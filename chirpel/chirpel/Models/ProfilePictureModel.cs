using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chirpel.Models
{
    public class ProfilePictureModel
    {
        public string Token { get; set; }
        public IFormFile Picture { get; set; }
    }
}
