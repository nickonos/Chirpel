using Chipel.Factory;
using Chirpel.Contract.Interfaces.Auth;
using Chirpel.Contract.Interfaces.DAL;
using Chirpel.Contract.Models.Post;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chirpel.Logic.Post
{
    public class PostLikesLogic
    {
        public string PostId { get; set; }
        public string UserId { get; set; }

        private readonly IPostLikesDAL _postLikesDAL;
        private readonly IAuthService _authService;

        public PostLikesLogic()
        {
            _postLikesDAL = Factory.CreateIPostLikesDAL();
            _authService = Factory.CreateIAuthService();
        }

        public List<string> GetLikes(string postId)
        {
            List<string> likes = new List<string>();
            List<PostLikes> postLikes = _postLikesDAL.GetPostLikes(postId);

            foreach(PostLikes like in postLikes)
            {
                likes.Add(like.UserId);
            }
            return likes;
        }
    }
}
