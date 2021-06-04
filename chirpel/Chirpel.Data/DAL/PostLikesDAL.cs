using Chirpel.Contract.Interfaces;
using Chirpel.Contract.Interfaces.DAL;
using Chirpel.Contract.Models.Post;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chirpel.Data.DAL
{
    public class PostLikesDAL : DAL<PostLikes>, IPostLikesDAL
    {

        public PostLikesDAL(DatabaseQuery databaseQuery) : base(databaseQuery)
        {
        }

        public List<PostLikes> GetPostLikes(string postId)
        {
            return _databaseQuery.Select<PostLikes>("PostId = @Value1", new string[] { postId});
        }

        public PostLikes GetPostLikes(string postId, string UserId)
        {
            return _databaseQuery.SelectFirst<PostLikes>("PostId = @Value1 And UserId = @Value2", new string[] { postId, UserId });
        }
    }
}
