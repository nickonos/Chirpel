using Chirpel.Factory;
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

        public PostLogic (IPostDAL postDAL)
        {
            _postDAL = postDAL;
            _authService = Factory.Factory.CreateIAuthService();
        }

        public PostLogic(IPostDAL postDAL, string content, string userId)
        {
            _postDAL = postDAL;
            Content = content;
            UserId = userId;
            _authService = Factory.Factory.CreateIAuthService();
        }

        public PostLogic()
        {
            _postDAL = Factory.Factory.CreateIPostDAL();
            _authService = Factory.Factory.CreateIAuthService();
        }

        public PostLogic(string content, string userId)
        {
            _postDAL = Factory.Factory.CreateIPostDAL();
            _authService = Factory.Factory.CreateIAuthService();
            Content = content;
            UserId = userId;
        }
        public PostLogic(string id, string content, string userId, DateTime postDate)
        {
            _postDAL = Factory.Factory.CreateIPostDAL();
            _authService = Factory.Factory.CreateIAuthService();
            Id = id;
            Content = content;
            UserId = userId;
            PostDate = postDate;
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
            if (Content == null || UserId == null)
                return;
            if (Id == null)
                Id = Guid.NewGuid().ToString();

            if (PostDate == DateTime.MinValue)
                PostDate = DateTime.UtcNow;


            _postDAL.Add(new Contract.Models.Post.Post(Id, Content, UserId, PostDate));
        }
    }
}
