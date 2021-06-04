using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chirpel.Contract.Interfaces;
using Chirpel.Contract.Interfaces.DAL;
using Chirpel.Contract.Models.Post;

namespace Chirpel.Data.FakeDAL
{
    public class FakePostDAL : IPostDAL
    {
        public List<Post> Posts { get; private set; }
        public List<Post> GetPostsOrderbyDesc()
        {
            return Posts.OrderBy(c => c.PostDate).ToList();
        }

        public List<Post> GetByUserId(string userId)
        {
            return Posts.FindAll(c => c.UserId == userId);
        }

        public Post Get(string id)
        {
            return Posts.Find(c => c.Id == id);
        }

        public List<Post> GetAll()
        {
            return Posts;
        }

        public void Add(Post entity)
        {
            Posts.Add(entity);
        }

        public void Remove(Post entity)
        {
            Posts.Remove(entity);
        }

        public void Update(Post entity)
        {
            Post post = Posts.Find(c => c.Id == entity.Id);
            if (entity.Content != null)
                post.Content = entity.Content;

            if (entity.PostDate != null)
                post.PostDate = entity.PostDate;

            if (entity.UserId != null)
                post.UserId = entity.UserId;
        }

        public FakePostDAL()
        {
            Posts = new List<Post>();
        }
    }
}
