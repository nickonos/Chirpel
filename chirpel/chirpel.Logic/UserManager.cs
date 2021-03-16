using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Security.Claims;
using Chirpel.Common.Models;
using Chirpel.Common.Models.Auth;
using Chirpel.Data;
using Chirpel.Logic.Auth;

namespace Chirpel.Logic
{
    public class UserManager
    {
        private readonly DatabaseQuery databaseQuery = new DatabaseQuery();
        private readonly IAuthService _authService;

        public UserManager(IAuthService authService)
        {
            _authService = authService;
        }

        public List<DBUser> GetAllUsers()
        {
            return databaseQuery.Select<DBUser>("User");
        }

        public HttpResponse Login(LoginUser user)
        {
            DBUser dbUser = databaseQuery.SelectFirst<DBUser>("User", $"Username= @Value1", new string[] { user.Username });

            if (dbUser == null)
                return new HttpResponse(false, "username");

            if (dbUser.Password == user.Password)
            {
                IAuthContainerModel model = new JWTContainerModel()
                {
                    Claims = new Claim[]
                {
                    new Claim(ClaimTypes.Name, dbUser.UserID)
                }
                };
                string token = _authService.GenerateToken(model);
                return new HttpResponse(true, token);
            }

            return new HttpResponse(false, "password");
        }

        public DBUser FindUser(string value, string column)
        {
            return databaseQuery.SelectFirst<DBUser>("User", $"{column}= @Value1", new string[] { value });
        }

        public bool VerifyUser(DBUser user)
        {
            if (databaseQuery.SelectFirst<DBUser>("User", $"Username= @Value1", new string[] { user.Username} ).Password == user.Password)
                return true;

            return false;
        }

        public HttpResponse AddUser(RegisterUser user)
        {
            Guid id = Guid.NewGuid();

            bool res = databaseQuery.Insert(new DBUser()
            {
                UserID = id.ToString(),
                Username = user.Username,
                Email = user.Email,
                Password = user.Password
            }, "User");

            if (!res)
                return new HttpResponse(false, "Error inserting into database");

            res = databaseQuery.Insert(new UserSettings {
            UserId = id,
            DarkModeEnabled = false,
            IsPrivate = false,
            Bio = "",
            ProfilePicture = ""
            }, "User_settings");
            
            if (!res)
                return new HttpResponse(false, "Error inserting into database");

            return new HttpResponse(true, "transaction succesful"); ;
        }

        public HttpResponse DeleteUser(DBUser user)
        {
            DBUser userCheck = databaseQuery.SelectFirst<DBUser>("User", $"Username= @Value1", new string[] { user.Username });

            if(userCheck!= null && user.Password == userCheck.Password)
            {
                Guid id = Guid.Parse(userCheck.UserID);
                bool res = databaseQuery.Delete("User_Followers", $"Followed= @Value1 OR Follower= @Value2", new string[] { id.ToString(), id.ToString() });
                if (!res)
                    return new HttpResponse(false, "Error deleting from database");

                res = databaseQuery.Delete("User_Settings", $"UserID= @Value1", new string[] { id.ToString() });
                if (!res)
                    return new HttpResponse(false, "Error deleting from database");

                res = databaseQuery.Delete("User", $"UserID= @Value1", new string[] { id.ToString() });
                if (!res)
                    return new HttpResponse(false, "Error deleting from database");

                return new HttpResponse(true, "transaction succesful");
            }

            return new HttpResponse(false, "usercredentials don't match");
        }

        public UserSettings GetSettings(string UserId)
        {
            return databaseQuery.SelectFirst<UserSettings>("User_settings", $"UserId= @Value1", new string[] { UserId });
        }

        public List<Guid> GetFollowers(string UserId)
        {
            List<Guid> followers = new List<Guid>();
            List<UserFollower> list = databaseQuery.Select<UserFollower>("User_Followers", $"Followed = @Value1", new string[] { UserId });
            foreach(UserFollower user in list)
                followers.Add(Guid.Parse(user.Follower));

            return followers;
        }

        public HttpResponse AddFollower(string UserId, string FollowerId)
        {
            DBUser user = databaseQuery.SelectFirst<DBUser>("User", $"UserId= @Value1", new string[] { UserId });

            if (user == null)
                return new HttpResponse(false, "User not found");

            DBUser Follower = databaseQuery.SelectFirst<DBUser>("User", $"UserId= @Value1", new string[] { FollowerId });
            if (Follower == null)
                return new HttpResponse(false, "Follower not found");

            UserFollower following = databaseQuery.SelectFirst<UserFollower>("User_followers", $"followed= @Value1 AND follower= @Value2", new string[] { UserId, FollowerId});

            if (following != null)
                return new HttpResponse(false, "already following this user");

            bool res = databaseQuery.Insert(new UserFollower() {Followed = UserId, Follower = FollowerId }, "User_Followers");

            if (!res)
                return new HttpResponse(false, "Error inserting into database");

            return new HttpResponse(true, "transaction succesful"); 
        }

        public HttpResponse RemoveFollower(string UserId, string FollowerId)
        {
            DBUser user = databaseQuery.SelectFirst<DBUser>("User", $"UserId= @Value1 ", new string[] { UserId });
            if (user == null)
                return new HttpResponse(false, "User not found");

            DBUser Follower = databaseQuery.SelectFirst<DBUser>("User", $"UserId= @Value1", new string[] { FollowerId});
            if (Follower == null)
                return new HttpResponse(false, "Follower not found");

            UserFollower following = databaseQuery.SelectFirst<UserFollower>("User_followers", $"followed= @Value1 AND follower= @Value2", new string[] { UserId, FollowerId });

            if (following == null)
                return new HttpResponse(false, "you aren't following this user");

            bool res = databaseQuery.Delete("User_followers", $"followed = @Value1 AND follower = @Value2", new string[] { UserId, FollowerId });

            if (!res)
                return new HttpResponse(false, "error deleting from database");

            return new HttpResponse(true, "transaction succesful");
        }

        public List<Guid> GetFollowing(string UserId)
        {
            List<Guid> following = new List<Guid>();
            List<UserFollower> list = databaseQuery.Select<UserFollower>("User_Followers", $"follower = @Value1", new string[] { UserId });

            foreach (UserFollower user in list)
                following.Add(Guid.Parse(user.Followed));
            
            return following;
        }
    }
}
