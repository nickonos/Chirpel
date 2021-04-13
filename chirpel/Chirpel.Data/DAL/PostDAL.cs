using System;
using System.Collections.Generic;
using System.Text;
using Chirpel.Common.Interfaces;
using Chirpel.Common.Models.Post;

namespace Chirpel.Data.DAL
{
    public class PostDAL : DAL<Post>, IPostDAL
    {
        public Post GetPostById(string id)
        {
            return _databaseQuery.SelectFirst<Post>("Post", "postid=@value1", new string[] { id });
        }

        public List<Post> GetPostsOrderbyDesc()
        {
            return _databaseQuery.Select<Post>("Post", "PostDate DESC");
        }

        public bool CreatePost(Post post)
        {
            return _databaseQuery.Insert<Post>(post, "Post");
        }

        public PostDAL(DatabaseQuery databaseQuery) : base(databaseQuery)
        {
        }
    }
}
