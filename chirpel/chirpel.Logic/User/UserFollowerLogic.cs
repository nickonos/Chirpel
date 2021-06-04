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
        public string Followed { get; private set; }
        public string Follower { get; private set; }

        private readonly IUserFollowersDAL _userFollowerDAL;
        private readonly IAuthService _authService;

        public UserFollowerLogic()
        {
            _userFollowerDAL = Factory.Factory.CreateIUserFollowerDAL();
            _authService = Factory.Factory.CreateIAuthService();
        }

        public UserFollowerLogic(IUserFollowersDAL userFollowersDAL)
        {
            _userFollowerDAL = userFollowersDAL;
            _authService = Factory.Factory.CreateIAuthService();
        }

        public UserFollowerLogic(IUserFollowersDAL userFollowersDAL, string followed, string follower)
        {
            Followed = followed;
            Follower = follower;
            _userFollowerDAL = userFollowersDAL;
            _authService = Factory.Factory.CreateIAuthService();
        }


        public UserFollowerLogic(string followed, string follower)
        {
            Followed = followed;
            Follower = follower;
            _userFollowerDAL = Factory.Factory.CreateIUserFollowerDAL();
            _authService = Factory.Factory.CreateIAuthService();
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

        public Response Add()
        {
            if (Followed == null || Follower == null)
                return new Response(false, "Invalid UserFollower");

            if (_userFollowerDAL.GetFollower(Followed, Follower) != null)
                return new Response(false, "you already follow this user"); ;

            _userFollowerDAL.Add(new UserFollowers() { Follower = Follower, Followed = Followed });
            return new Response(true, "transaction succesful");
        }

        public void Remove()
        {
            if (Followed == null || Follower == null)
                return;

            if (_userFollowerDAL.GetFollower(Followed, Follower) == null)
                return;

            _userFollowerDAL.Remove(new UserFollowers() { Followed = Followed, Follower = Follower });
        }
    }
}
