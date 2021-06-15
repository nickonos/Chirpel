using Chirpel.Contract.Interfaces.DAL;
using Chirpel.Contract.Models.Message;
using Chirpel.Logic.Account;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chirpel.Logic.Message
{
    public class PostCollection
    {
        public List<PostLogic> Posts { get; private set; }

        private readonly IPostDAL _postDAL;
        private readonly IPostLikesDAL _postLikesDAL;

        public PostCollection()
        {
            _postDAL = Factory.Factory.CreateIPostDAL();
            _postLikesDAL = Factory.Factory.CreateIPostLikesDAL();
            Posts = new List<PostLogic>();
        }

        public PostCollection(IPostDAL postDAL, IPostLikesDAL postLikesDAL)
        {
            _postDAL = postDAL;
            _postLikesDAL = postLikesDAL;
            Posts = new List<PostLogic>();
        }


        public void GetExplore()
        {
            List<Post> posts = _postDAL.GetPostsOrderbyDesc();
            int i = 0;
            foreach(Post post in posts)
            {
                if (i == 10)
                    return;
                
                UserLogic userLogic = new UserLogic();
                userLogic.GetById(post.UserId);
                
                if (userLogic.IsPrivate)
                    continue;
       
                Posts.Add(new PostLogic(post.Id, post.Content, userLogic, post.PostDate));
                i++;
            }
        }

        public void GetPersonal(string userId)
        {
            List<Post> posts = _postDAL.GetPostsOrderbyDesc();
            int i = 0;
            UserLogic userLogic1 = new UserLogic();
            userLogic1.GetById(userId);

            foreach (Post post in posts)
            {
                if (i == 10)
                    return;

                UserLogic userLogic = new UserLogic();
                userLogic.GetById(post.UserId);

                if (!userLogic1.CheckIfFollows(userLogic))
                    continue;

                Posts.Add(new PostLogic(post.Id, post.Content, userLogic, post.PostDate));
                i++;
            }
        }

        public void GetAllPostsFromUser(string id)
        {
            List<Post> posts = _postDAL.GetByUserId(id);

            foreach (Post post in posts)
            {
                UserLogic userLogic = new UserLogic();
                userLogic.GetById(post.UserId);
                Posts.Add(new PostLogic(post.Id, post.Content, userLogic, post.PostDate));
            }
        }

        public void RemoveAllPostFromUser(string id)
        {
            List<Post> posts = _postDAL.GetByUserId(id);

            foreach (Post post in posts)
            {
                PostLogic postLogic = new PostLogic();
                postLogic.RemoveLikesFromPost(post.Id);
                _postDAL.Remove(post);
            }
        }

        public Response AddPost(string content, UserLogic user)
        {
            if (content == null || user == null)
                return new Response(false, "invalid parameters");

            string id = Guid.NewGuid().ToString();
            DateTime dateTime = DateTime.UtcNow;
            PostLogic postLogic = new PostLogic(id, content, user, dateTime);
            Posts.Add(postLogic);

            _postDAL.Add(new Post() { Id = id, Content = content, UserId = user.Id, PostDate = dateTime });

            return new Response(true, id);
        }

        public Response DeletePost(PostLogic postLogic)
        {
            if (postLogic.Id != null)
                return new Response(false, "Id is null");

            Post post = new Post();

            if (postLogic.Content != null)
                post.Content = postLogic.Content;

            if (postLogic.PostDate != null)
                post.PostDate = postLogic.PostDate;

            if (postLogic.User.Id != null)
                post.UserId = postLogic.User.Id;

            _postDAL.Remove(post);
            return new Response(true, "post added");
        }

        public void RemoveLikesFromUser(string userId)
        {
            List<PostLikes> postLikes = _postLikesDAL.GetPostLikesFromUser(userId);

            foreach (PostLikes postLike in postLikes)
            {
                _postLikesDAL.Remove(postLike);
            }
        }
    }
}
