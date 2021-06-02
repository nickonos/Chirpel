using Chirpel.Contract.Interfaces.DAL;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chirpel.Logic.User
{
    public class UserFollowerCollection
    {
        public List<UserFollowerLogic> userFollowers {get; private set;}
        private readonly IUserFollowersDAL _userFollowerDAL;

        public UserFollowerCollection()
        {
            userFollowers = new List<UserFollowerLogic>();
            _userFollowerDAL = Factory.Factory.CreateIUserFollowerDAL();
        }
        public void Remove(string UserId)
        {
            _userFollowerDAL.DeleteAll(UserId);
        }

    }
}
