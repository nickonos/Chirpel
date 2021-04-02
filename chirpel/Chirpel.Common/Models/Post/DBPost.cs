using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chirpel.Common.Models.Post
{
    public class DBPost
    {
        public string PostId { get; set; }
        public string Content { get; set; }
        public string UserId { get; set; }
        public DateTime PostDate { get; set; }
    }
}
