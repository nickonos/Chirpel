using Chipel.Factory;
using Chirpel.Contract.Interfaces.Auth;
using Chirpel.Contract.Interfaces.DAL;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chirpel.Logic.Post
{
    public class PostLogic
    {
        public string Id { get; private set; }
        public string Content { get; private set; }
        public string UserId { get; private set; }
        public DateTime PostDate { get; private set; }

        private readonly IPostDAL _postDAL;
        private readonly IAuthService _authService;

        public PostLogic()
        {
            _postDAL = Factory.CreateIPostDAL();
            _authService = Factory.CreateIAuthService();
        }

        public PostLogic(string id, string content, string userId, DateTime postDate)
        {
            Id = id;
            Content = content;
            UserId = userId;
            PostDate = postDate;
        }

        public PostLogic(string content, string userId)
        {
            Content = content;
            UserId = userId;
        }

        public void GetById(string id)
        {
            Contract.Models.Post.Post post = _postDAL.Get(id);

            if (post == null)
                return;
                
            Id = post.Id;
            Content = post.Content;
            UserId = post.UserId;
            PostDate = post.PostDate;
        }

        public List<string> GetAllPostsFromUser(string id)
        {
            List<string> output = new List<string>();

            List<Contract.Models.Post.Post> posts = _postDAL.GetByUserId(id);

            foreach(Contract.Models.Post.Post post in posts)
            {
                output.Add(post.Id);
            }
            return output;
        }

        public void Add()
        {
            if (Id == null)
                Id = Guid.NewGuid().ToString();

            if (PostDate == null)
                PostDate = DateTime.UtcNow;

            _postDAL.Add(new Contract.Models.Post.Post(Id, Content, UserId, PostDate));
        }
    }
}
