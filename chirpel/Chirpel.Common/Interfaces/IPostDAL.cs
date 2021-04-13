using Chirpel.Common.Models.Post;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chirpel.Common.Interfaces
{
    public interface IPostDAL : IDAL<Post>
    {
        List<Post> GetPostsOrderbyDesc();
        bool CreatePost(Post post);
    }
}
