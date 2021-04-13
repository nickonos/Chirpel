using Chirpel.Common.Models.Account;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chirpel.Common.Interfaces
{
    public interface IUserFollowersDAL : IDAL<UserFollower>
    {
        UserFollower GetFollowers(string UserId, string Follower);
    }
}
