using Chirpel.Factory;
using Chirpel.Contract.Interfaces.Auth;
using Chirpel.Contract.Interfaces.DAL;
using System;
using System.Collections.Generic;
using System.Text;
using Chirpel.Logic.Account;
using Chirpel.Contract.Models.Message;

namespace Chirpel.Logic.Message
{
    public class PostLogic
    {
        public string Id { get; private set; }
        public string Content { get; private set; }
        public UserLogic User { get; private set; }
        public DateTime PostDate { get; private set; }
        public List<UserLogic> Likes { get; private set; }

        private readonly IPostDAL _postDAL;
        private readonly IPostLikesDAL _postLikesDal;

        public PostLogic (IPostDAL postDAL, IPostLikesDAL postLikesDAL)
        {
            _postDAL = postDAL;
            _postLikesDal = postLikesDAL;
            Likes = new List<UserLogic>();
        }

        public PostLogic()
        {
            _postDAL = Factory.Factory.CreateIPostDAL();
            _postLikesDal = Factory.Factory.CreateIPostLikesDAL();
            Likes = new List<UserLogic>();
        }

        public PostLogic(string id, string content, UserLogic user, DateTime postDate)
        {
            _postDAL = Factory.Factory.CreateIPostDAL();
            _postLikesDal = Factory.Factory.CreateIPostLikesDAL();
            Likes = new List<UserLogic>();
            Id = id;
            Content = content;
            User = user;
            PostDate = postDate;
        }

        public void GetById(string id)
        {
            Post post = _postDAL.Get(id);

            if (post == null)
                return;
                
            Id = post.Id;
            Content = post.Content;
            UserLogic user = new UserLogic();
            user.GetById(post.Id);
            User = user;
            PostDate = post.PostDate;
        }

        public void RemoveLikesFromPost(string postId)
        {
            List<PostLikes> postLikes = _postLikesDal.GetPostLikes(postId);

            foreach (PostLikes postLike in postLikes)
            {
                _postLikesDal.Remove(postLike);
            }
        }

        public Response RemoveLikes(UserLogic userLogic)
        {
            if (Likes.Contains(userLogic))
            {
                Likes.Remove(userLogic);
                _postLikesDal.Remove(new PostLikes() { PostId = Id, UserId = userLogic.Id });
                return new Response(true, "postlike removed");
            }
            return new Response(false, "postlikes doesnt contain user");
        }

        public Response Like(UserLogic userLogic)
        {
            if (!Likes.Contains(userLogic))
            {
                Likes.Add(userLogic);
                _postLikesDal.Add(new PostLikes() { PostId = Id, UserId = userLogic.Id });
                return new Response(true, "postliked");
            }
            return new Response(false, "postlikes contains user");
        }
    }
}
