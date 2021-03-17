using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chirpel.Common.Models.Account
{
    public class ProfilePictureModel
    {
        public string token { get; set; }
        public IFormFile picture { get; set; }
    }
}
