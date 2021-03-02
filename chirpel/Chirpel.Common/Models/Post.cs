using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chirpel.Common.Models
{
    public class Post
    {
        public Guid id { get; set; }
        public string Content { get; set; }
        public Guid UserId { get; set; }
        public DateTime PostTime { get; set; }
    }
}
