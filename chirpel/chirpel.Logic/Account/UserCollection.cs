using Chirpel.Contract.Interfaces.Auth;
using Chirpel.Contract.Interfaces.DAL;
using Chirpel.Contract.Models.Account;
using Chirpel.Contract.Models.Auth;
using Chirpel.Logic.Account;
using Chirpel.Logic.Message;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace Chirpel.Logic.Account
{
    public class UserCollection
    {
        public List<UserLogic> Users { get; private set; }

        private readonly IUserDAL _userDAL;
        private readonly IUserSettingsDAL _userSettingsDAL;
        private readonly IUserFollowersDAL _userFollowersDAL;

        public UserCollection()
        {
            _userDAL = Factory.Factory.CreateIUserDAL();
            _userSettingsDAL = Factory.Factory.CreateIUserSettingsDAL();
            _userFollowersDAL = Factory.Factory.CreateIUserFollowerDAL();
            Users = new List<UserLogic>();
        }

        public UserCollection(IUserDAL userDAL, IUserFollowersDAL userFollowersDAL, IUserSettingsDAL userSettingsDAL)
        {
            _userDAL = userDAL;
            _userFollowersDAL = userFollowersDAL;
            _userSettingsDAL = userSettingsDAL;
            Users = new List<UserLogic>();
        }

        public void GetAll()
        {
            List<User> users = _userDAL.GetAll();

            foreach(User user in users)
            {
                UserSettings userSettings = _userSettingsDAL.Get(user.Id);
                Users.Add(new UserLogic(user.Username, user.Email, user.Password, user.Id, userSettings.DarkModeEnabled, userSettings.IsPrivate, userSettings.Bio, userSettings.ProfilePicture));
            }
        }

        public Response Register(string Username, string Email, string Password)
        {
            UserLogic user = new UserLogic();
            user.GetByUsername(Username);
            
            if (user.Id != null)
                return new Response(false, "username");

            user.GetByEmail(Email);
            if (user.Id != null)
                return new Response(false, "email");

            string Id = Guid.NewGuid().ToString();

           
            _userDAL.Add(new User() { Id = Id, Username = Username, Email = Email, Password = Password });
            _userSettingsDAL.Add(new UserSettings() { Id = Id, Bio = "", ProfilePicture = "", DarkModeEnabled = false, IsPrivate = false });

            return new Response(true, "User Registered");
        }

        public Response RemoveUser(string userId)
        {
            UserLogic userLogic = new UserLogic();
            userLogic.GetById(userId);
            if (userLogic.Id == null)
                return new Response(false, "user not found");

            PostCollection postCollection = new PostCollection();
            postCollection.RemoveAllPostFromUser(userId);
            postCollection.RemoveLikesFromUser(userId);
            
            _userFollowersDAL.DeleteAll(userId);

            _userSettingsDAL.Remove(new UserSettings() { Id = userLogic.Id, Bio = userLogic.Bio, DarkModeEnabled = userLogic.DarkModeEnabled, IsPrivate = userLogic.IsPrivate, ProfilePicture = userLogic.ProfilePicture });
            
            _userDAL.Remove(new User() { Username = userLogic.Username, Id = userLogic.Id, Email = userLogic.Email, Password = userLogic.Password});
            return new Response(true, "User removed");
        }

        public void GetFollowers(string userId)
        {
            List<UserFollowers> userFollowers = _userFollowersDAL.GetFollowers(userId);
            foreach(UserFollowers userFollower in userFollowers)
            {
                UserLogic userLogic = new UserLogic();
                userLogic.GetById(userFollower.Follower);
                Users.Add(userLogic);
            }
        }
    }
}
