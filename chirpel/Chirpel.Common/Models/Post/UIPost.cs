using System;
using System.Collections.Generic;
using System.Text;

namespace Chirpel.Common.Models.Post
{
    public class UIPost
    {
        public string PostId { get; set; }
        public string Content { get; set; }
        public string UserId { get; set; }
        public DateTime PostDate { get; set; }
        public List<string> Likes { get; set; }
        public List<string> Comments { get; set; }
        public string Userpfp { get; set; }
        public string Username { get; set; }
    }
}
