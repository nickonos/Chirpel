using Chirpel.Contract.Models.Post;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chirpel.Contract.Interfaces.DAL
{
    public interface IPostDAL : IDAL<Post>
    {
        List<Post> GetPostsOrderbyDesc();
        bool CreatePost(Post post);
        List<Post> GetByUserId(string userId);
    }
}
