using Chirpel.Contract.Interfaces;
using Chirpel.Contract.Interfaces.DAL;
using Chirpel.Contract.Models.Account;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chirpel.Data.DAL
{
    public class UserFollowersDAL : DAL<UserFollowers>, IUserFollowersDAL
    {
        public UserFollowers GetFollower(string UserId, string Follower)
        {
            return _databaseQuery.SelectFirst<UserFollowers>("followed=@value1 and follower = @value2", new string[] {UserId, Follower });
        }

        public List<UserFollowers> GetFollowers(string UserId)
        {
            return _databaseQuery.Select<UserFollowers>("Followed=@value1", new string[] { UserId });
        }

        public List<UserFollowers> GetFollowing(string UserId)
        {
            return _databaseQuery.Select<UserFollowers>("Follower=@value1", new string[] { UserId });
        }
        
        public void DeleteAll(string UserId)
        {
            _databaseQuery.Delete("UserFollowers", "Follower=@value1 OR Followed=@value2", new string[] { UserId, UserId });
        }

        public UserFollowersDAL(DatabaseQuery databaseQuery) : base(databaseQuery)
        {
        }
    }
}
