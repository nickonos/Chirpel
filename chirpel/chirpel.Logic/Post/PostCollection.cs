using System;
using System.Collections.Generic;
using System.Text;

namespace Chirpel.Logic.Post
{
    public class PostCollection
    {
        public List<PostLogic> Posts { get; private set; }

        public PostCollection()
        {
            Posts = new List<PostLogic>();
        }
    }
}
