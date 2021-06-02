using Chirpel.Contract.Models.Account;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chirpel.Contract.Interfaces.DAL
{
    public interface IUserFollowersDAL : IDAL<UserFollowers>
    {
        UserFollowers GetFollower(string UserId, string Follower);
        List<UserFollowers> GetFollowers(string UserId);
        List<UserFollowers> GetFollowing(string UserId);
        void DeleteAll(string UserId);
    }
}
