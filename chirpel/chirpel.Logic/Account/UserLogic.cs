using Chirpel.Contract.Interfaces.Auth;
using Chirpel.Contract.Interfaces.DAL;
using Chirpel.Contract.Models.Auth;
using Chirpel.Contract.Models.Message;
using Chirpel.Logic.Message;
using Chirpel.Logic.Account;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using Chirpel.Contract.Models.Account;

namespace Chirpel.Logic.Account
{
    public class UserLogic
    {
        public string Id { get; private set; }
        public string Username { get; private set; }
        public string Email { get; private set; }
        public string Password { get; private set; }
        public bool DarkModeEnabled { get; private set; }
        public bool IsPrivate { get; private set; }
        public string Bio { get; private set; }
        public string ProfilePicture { get; private set; }
        public List<UserLogic> Following { get; private set; }

        private readonly IUserDAL _userDAL;
        private readonly IAuthService _authService;
        private readonly IUserFollowersDAL _userFollowerDAL;
        private readonly IUserSettingsDAL _userSettingsDAL;

        public UserLogic()
        {
            _userDAL = Factory.Factory.CreateIUserDAL();
            _userFollowerDAL = Factory.Factory.CreateIUserFollowerDAL();
            _userSettingsDAL = Factory.Factory.CreateIUserSettingsDAL();
            _authService = Factory.Factory.CreateIAuthService();
            Following = new List<UserLogic>();
        }

        public UserLogic(IUserDAL userDAl, IUserFollowersDAL userFollowersDAL, IUserSettingsDAL userSettingsDAL)
        {
            _userDAL = userDAl;
            _userFollowerDAL = userFollowersDAL;
            _userSettingsDAL = userSettingsDAL;
            _authService = Factory.Factory.CreateIAuthService();
            Following = new List<UserLogic>();
        }

        public UserLogic(IUserDAL userDAl ,string username, string email, string password)
        {
            Username = username;
            Email = email;
            Password = password;
            _userDAL = userDAl;
            _authService = Factory.Factory.CreateIAuthService();
            Following = new List<UserLogic>();
        }

        public UserLogic(string username, string email, string password, string id, bool darkmode, bool _private, string bio, string pfp)
        {
            Id = id;
            Username = username;
            Email = email;
            Password = password;
            DarkModeEnabled = darkmode;
            IsPrivate = _private;
            Bio = bio;
            ProfilePicture = pfp;
            _userDAL = Factory.Factory.CreateIUserDAL();
            _authService = Factory.Factory.CreateIAuthService();
            _userFollowerDAL = Factory.Factory.CreateIUserFollowerDAL();
            _userSettingsDAL = Factory.Factory.CreateIUserSettingsDAL();
            Following = new List<UserLogic>();
        }

        public UserLogic(string username, string password)
        {
            Username = username;
            Password = password;
            _userDAL = Factory.Factory.CreateIUserDAL();
            _userFollowerDAL = Factory.Factory.CreateIUserFollowerDAL();
            _userSettingsDAL = Factory.Factory.CreateIUserSettingsDAL();
            _authService = Factory.Factory.CreateIAuthService();
            Following = new List<UserLogic>();
        }

        public UserLogic(string username, string email, string password)
        {
            Username = username;
            Email = email;
            Password = password;
            _userDAL = Factory.Factory.CreateIUserDAL();
            _authService = Factory.Factory.CreateIAuthService();
            _userFollowerDAL = Factory.Factory.CreateIUserFollowerDAL();
            _userSettingsDAL = Factory.Factory.CreateIUserSettingsDAL();
            Following = new List<UserLogic>();
        } 

        public Response Verify()
        {
            User user = _userDAL.GetByUsername(Username);

            if (user == null)
                return new Response(false, "username");

            if (user.Password != Password)
                return new Response(false, "password");

            Id = user.Id;

            return new Response(true, "verified user");
        }

        public void GetByUsername(string username)
        {
            if (_userDAL.GetByUsername(username) == null)
                return;

            User user = _userDAL.GetByUsername(username);
            Id = user.Id;
            Username = user.Username;
            Email = user.Email;
            Password = user.Password;
            UserSettings userSettings = _userSettingsDAL.Get(Id);
            Bio = userSettings.Bio;
            ProfilePicture = userSettings.ProfilePicture;
            DarkModeEnabled = userSettings.DarkModeEnabled;
            IsPrivate = userSettings.IsPrivate;
        }

        public void GetByEmail(string email)
        {
            User user = _userDAL.GetByEmail(email);

            if (user == null)
                return;

            Id = user.Id;
            Username = user.Username;
            Email = user.Email;
            Password = user.Password;
            UserSettings userSettings = _userSettingsDAL.Get(Id);
            Bio = userSettings.Bio;
            ProfilePicture = userSettings.ProfilePicture;
            DarkModeEnabled = userSettings.DarkModeEnabled;
            IsPrivate = userSettings.IsPrivate;
        }

        public void GetById(string id)
        {
            User user = _userDAL.Get(id);

            if (user == null)
                return;

            Id = user.Id;
            Username = user.Username;
            Email = user.Email;
            Password = user.Password;
            UserSettings userSettings = _userSettingsDAL.Get(id);
            Bio = userSettings.Bio;
            ProfilePicture = userSettings.ProfilePicture;
            DarkModeEnabled = userSettings.DarkModeEnabled;
            IsPrivate = userSettings.IsPrivate;
        }

        public void GetFollowing()
        {
            if (Id == null)
                return;

            List<UserFollowers> userFollowers = _userFollowerDAL.GetFollowing(Id);
            foreach(UserFollowers user in userFollowers)
            {
                UserLogic userLogic = new UserLogic();
                userLogic.GetById(user.Followed);
                Following.Add(userLogic);
            }
        }

        public void GetByPassword(string password)//For test purposes only
        {
            User user = _userDAL.GetByPassword(password);

            if (user == null)
                return;

            Id = user.Id;
            Username = user.Username;
            Email = user.Email;
            Password = user.Password;
            UserSettings userSettings = _userSettingsDAL.Get(Id);
            Bio = userSettings.Bio;
            ProfilePicture = userSettings.ProfilePicture;
            DarkModeEnabled = userSettings.DarkModeEnabled;
            IsPrivate = userSettings.IsPrivate;
        }

        public Response Login()
        {
            Response res = Verify();

            if (!res.Succes)
                return res;

            IAuthContainerModel model = new JWTContainerModel()
            {
                Claims = new Claim[]
                {
                    new Claim(ClaimTypes.Name, Id)
                }
            };
            string token = _authService.GenerateToken(model);

            return new Response(true, token);
        }

        public Response FollowUser(UserLogic user)
        {
            if (_userFollowerDAL.GetFollower(user.Id, Id) != null)
                return new Response(false, "you already follow this user"); ;

            _userFollowerDAL.Add(new UserFollowers() { Follower = Id, Followed = user.Id });
            return new Response(true, "transaction succesful");
        }

        public Response UnfollowUser(UserLogic user)
        {
            if (_userFollowerDAL.GetFollower(user.Id, Id) == null)
                return new Response(false, "you don't follow this user");

            _userFollowerDAL.Remove(new UserFollowers() { Followed = Id, Follower = Id });
            return new Response(true, "user unfollowed");
        }

        public bool CheckIfFollows(UserLogic user)
        {
            UserFollowers userFollowers = _userFollowerDAL.GetFollower(user.Id, Id);

            return userFollowers != null;
        }

        public void UpdateBio(string bio)
        {
            if (Id == null)
                return;

            _userSettingsDAL.Update(new UserSettings() { Id = Id, Bio = bio });
        }
    }
}
