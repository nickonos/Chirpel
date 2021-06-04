using Chirpel.Contract.Interfaces;
using Chirpel.Contract.Interfaces.DAL;
using Chirpel.Contract.Models.Account;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chirpel.Data.FakeDAL
{
    public class FakeUserFollowersDAL : IUserFollowersDAL
    {
        public List<UserFollowers> userFollowers { get; private set; }

        public UserFollowers GetFollower(string UserId, string Follower)
        {
            return userFollowers.Find(c => c.Followed == UserId && c.Follower == Follower);
        }

        public List<UserFollowers> GetFollowers(string UserId)
        {
            return userFollowers.FindAll(c => c.Followed == UserId);
        }

        public List<UserFollowers> GetFollowing(string UserId)
        {
            return userFollowers.FindAll(c => c.Follower == UserId);
        }
        
        public void DeleteAll(string UserId)
        {
            userFollowers.RemoveAll(c => c.Follower == UserId);
            userFollowers.RemoveAll(c => c.Followed == UserId);
        }

        public UserFollowers Get(string id)
        {
            throw new Exception("This method should not be called by join table");
        }

        public List<UserFollowers> GetAll()
        {
            return userFollowers;
        }

        public void Add(UserFollowers entity)
        {
            userFollowers.Add(entity);
        }

        public void Remove(UserFollowers entity)
        {
            userFollowers.Remove(entity);
        }

        public void Update(UserFollowers entity)
        {
            throw new Exception("This method should not be called by join table");
        }

        public FakeUserFollowersDAL()
        {
            userFollowers = new List<UserFollowers>();
        }
    }
}
