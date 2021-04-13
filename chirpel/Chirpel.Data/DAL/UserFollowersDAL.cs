using Chirpel.Common.Interfaces;
using Chirpel.Common.Models.Account;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chirpel.Data.DAL
{
    public class UserFollowersDAL : DAL<UserFollower>, IUserFollowersDAL
    {
        public UserFollower GetFollowers(string UserId, string Follower)
        {
            throw new NotImplementedException();
        }

        public UserFollowersDAL(DatabaseQuery databaseQuery) : base(databaseQuery)
        {
        }
    }
}
