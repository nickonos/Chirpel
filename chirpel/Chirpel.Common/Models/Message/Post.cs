using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chirpel.Contract.Models.Message
{
    public class Post
    {
        public string Id { get; set; }
        public string Content { get; set; }
        public string UserId { get; set; }
        public DateTime PostDate { get; set; }

        public Post() { }

        public Post(string id, string content, string userId, DateTime postDate)
        {
            Id = id;
            Content = content;
            UserId = userId;
            PostDate = postDate;
        }
    }
}
