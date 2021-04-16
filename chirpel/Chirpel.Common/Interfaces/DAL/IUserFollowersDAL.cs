using Chirpel.Common.Models.Account;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chirpel.Common.Interfaces.DAL
{
    public interface IUserFollowersDAL : IDAL<UserFollowers>
    {
        UserFollowers GetFollowers(string UserId, string Follower);
        List<UserFollowers> GetFollowers(string UserId);
        List<UserFollowers> GetFollowing(string UserId);
    }
}
