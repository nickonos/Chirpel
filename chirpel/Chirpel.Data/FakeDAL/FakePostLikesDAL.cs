using Chirpel.Contract.Interfaces;
using Chirpel.Contract.Interfaces.DAL;
using Chirpel.Contract.Models.Post;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chirpel.Data.FakeDAL
{
    public class FakePostLikesDAL : IPostLikesDAL
    {
        public List<PostLikes> PostLikes { get; private set; }

        public void Add(PostLikes entity)
        {
            PostLikes.Add(entity);
        }

        public PostLikes Get(string id)
        {
            throw new Exception("This method should not be called by join table");
        }

        public List<PostLikes> GetAll()
        {
            return PostLikes;
        }

        public List<PostLikes> GetPostLikes(string postId)
        {
            return PostLikes.FindAll(c => c.PostId == postId);
        }

        public void Remove(PostLikes entity)
        {
            PostLikes.Remove(entity);
        }

        public void Update(PostLikes entity)
        {
            throw new Exception("This method should not be called by join table");
        }

        public PostLikes GetPostLikes(string postId, string UserId)
        {
            return PostLikes.Find(c => c.PostId == postId && c.UserId == UserId);
        }

        public List<PostLikes> GetPostLikesFromUser(string userId)
        {
            throw new NotImplementedException();
        }

        public FakePostLikesDAL()
        {
            PostLikes = new List<PostLikes>();
        }
    }
}
