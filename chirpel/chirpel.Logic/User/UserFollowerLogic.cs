using Chirpel.Factory;
using Chirpel.Contract.Interfaces.Auth;
using Chirpel.Contract.Interfaces.DAL;
using Chirpel.Contract.Models.Account;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chirpel.Logic.User
{
    public class UserFollowerLogic
    {
        public string Followed { get; set; }
        public string Follower { get; set; }

        private readonly IUserFollowersDAL _userFollowerDAL;
        private readonly IAuthService _authService;

        public UserFollowerLogic()
        {
            _userFollowerDAL = Factory.Factory.CreateIUserFollowerDAL();
            _authService = Factory.Factory.CreateIAuthService();
        }

        public UserFollowerLogic(string followed, string follower)
        {
            Followed = followed;
            Follower = follower;
        } 
        public List<string> GetFollowers(string id)
        {
            List<string> list = new List<string>();
            List<UserFollowers> users = _userFollowerDAL.GetFollowers(id);

            foreach(UserFollowers user in users)
            {
                list.Add(user.Follower);
            }
            return list;
        }

        public List<string> GetFollowing(string id)
        {
            List<string> list = new List<string>();
            List<UserFollowers> users = _userFollowerDAL.GetFollowing(id);

            foreach (UserFollowers user in users)
            {
                list.Add(user.Followed);
            }
            return list;
        }

        public void GetPair(string following, string follower)
        {
            UserFollowers userFollowers = _userFollowerDAL.GetFollower(following, follower);
            if (userFollowers == null)
                return;

            Followed = userFollowers.Followed;
            Follower = userFollowers.Follower;
        }
        public void Add()
        {
            throw new NotImplementedException();
        }

        public void Remove()
        {
            throw new NotImplementedException();
        }
    }
}
