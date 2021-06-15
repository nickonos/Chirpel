using Chirpel.Factory;
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
        public string PostId { get; private set; }
        public string UserId { get; private set; }

        private readonly IPostLikesDAL _postLikesDAL;
        private readonly IAuthService _authService;

        public PostLikesLogic()
        {
            _postLikesDAL = Factory.Factory.CreateIPostLikesDAL();
            _authService = Factory.Factory.CreateIAuthService();
        }

        public PostLikesLogic(IPostLikesDAL post)
        {
            _postLikesDAL = post;
            _authService = Factory.Factory.CreateIAuthService();
        }

        public PostLikesLogic(IPostLikesDAL post, string postId, string userId)
        {
            PostId = postId;
            UserId = userId;
            _postLikesDAL = post;
            _authService = Factory.Factory.CreateIAuthService();
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

        public void Add()
        {
            if (PostId == null || UserId == null)
                return;

            if (_postLikesDAL.GetPostLikes(PostId, UserId) != null)
                return;

            _postLikesDAL.Add(new PostLikes() { PostId = PostId, UserId = UserId });
        }

        public void RemoveLikesFromUser(string userId)
        {
            List<PostLikes> postLikes = _postLikesDAL.GetPostLikesFromUser(userId);

            foreach(PostLikes postLike in postLikes)
            {
                _postLikesDAL.Remove(postLike);
            }
        }

        public void RemoveLikesFromPost(string postId)
        {
            List<PostLikes> postLikes = _postLikesDAL.GetPostLikes(postId);

            foreach (PostLikes postLike in postLikes)
            {
                _postLikesDAL.Remove(postLike);
            }
        }
    }
}
