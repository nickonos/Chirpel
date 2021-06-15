using System;
using System.Collections.Generic;
using System.Text;

namespace Chirpel.Contract.Models.Message
{
    public class PostLikes
    {
        public string PostId { get; set; }
        public string UserId { get; set; }
    }
}
