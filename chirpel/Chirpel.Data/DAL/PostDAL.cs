using System;
using System.Collections.Generic;
using System.Text;
using Chirpel.Common.Interfaces;
using Chirpel.Common.Interfaces.DAL;
using Chirpel.Common.Models.Post;

namespace Chirpel.Data.DAL
{
    public class PostDAL : DAL<Post>, IPostDAL
    {
        public List<Post> GetPostsOrderbyDesc()
        {
            return _databaseQuery.Select<Post>("PostDate DESC");
        }

        public bool CreatePost(Post post)
        {
            return _databaseQuery.Insert(post);
        }

        public PostDAL(DatabaseQuery databaseQuery) : base(databaseQuery)
        {
        }
    }
}
