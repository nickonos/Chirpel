using Chirpel.Contract.Interfaces.DAL;
using Chirpel.Logic.User;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chirpel.Logic.Post
{
    public class PostCollection
    {
        public List<PostLogic> Posts { get; private set; }

        private readonly IPostDAL _postDAL;

        public PostCollection()
        {
            _postDAL = Factory.Factory.CreateIPostDAL();
            Posts = new List<PostLogic>();
        }

        public PostCollection(IPostDAL postDAL)
        {
            _postDAL = postDAL;
            Posts = new List<PostLogic>();
        }


        public void GetExplore()
        {
            List<Contract.Models.Post.Post> posts = _postDAL.GetPostsOrderbyDesc();
            int i = 0;
            foreach(Contract.Models.Post.Post post in posts)
            {
                if (i == 10)
                    return;

                UserSettingsLogic userSettings = new UserSettingsLogic();
                userSettings.GetById(post.UserId);

                if (userSettings.IsPrivate)
                    continue;

                Posts.Add(new PostLogic(post.Id, post.Content, post.UserId, post.PostDate));
                i++;
            }
        }

        public void GetPersonal(string userId)
        {
            List<Contract.Models.Post.Post> posts = _postDAL.GetPostsOrderbyDesc();
            int i = 0;
            
            foreach (Contract.Models.Post.Post post in posts)
            {
                if (i == 10)
                    return;

                UserFollowerLogic userFollowerLogic = new UserFollowerLogic();
                userFollowerLogic.GetPair(post.UserId, userId);

                if (userFollowerLogic.Followed == null)
                    continue;

                Posts.Add(new PostLogic(post.Id, post.Content, post.UserId, post.PostDate));
                i++;
            }
        }

        public void GetAllPostsFromUser(string id)
        {
            List<Contract.Models.Post.Post> posts = _postDAL.GetByUserId(id);

            foreach (Contract.Models.Post.Post post in posts)
            {
                Posts.Add(new PostLogic(post.Id, post.Content, post.UserId, post.PostDate));
            }
        }
    }
}
