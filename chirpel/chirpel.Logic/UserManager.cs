using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Security.Claims;
using Chirpel.Common.Models;
using Chirpel.Common.Models.Account;
using Chirpel.Common.Models.Auth;
using Chirpel.Data;
using Chirpel.Logic.Auth;
using Microsoft.AspNetCore.Http;

namespace Chirpel.Logic
{
    public class UserManager
    {
        private readonly DatabaseQuery _databaseQuery = new DatabaseQuery();
        private readonly IAuthService _authService;

        public UserManager(IAuthService authService)
        {
            _authService = authService;
        }

        public List<DBUser> GetAllUsers()
        {
            return _databaseQuery.Select<DBUser>("User");
        }

        public ApiResponse Login(LoginUser user)
        {
            DBUser dbUser = _databaseQuery.SelectFirst<DBUser>("User", $"Username= @Value1", new string[] { user.Username });

            if (dbUser == null)
                return new ApiResponse(false, "username");

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
                return new ApiResponse(true, token);
            }

            return new ApiResponse(false, "password");
        }

        public UIAccount GetUIAccount(string UserId)
        {
            DBUser user = _databaseQuery.SelectFirst<DBUser>("User", "UserId= @Value1", new string[] { UserId });
            if (user == null)
                return new UIAccount();

            UserSettings settings = _databaseQuery.SelectFirst<UserSettings>("User_settings", "UserId= @value1", new string[] { UserId });
            if (user == null)
                return new UIAccount();

            List<Guid> followers = GetFollowers(UserId);

            List<Guid> following = GetFollowing(UserId);

            string pfp = settings.ProfilePicture.Replace("C:\\Users\\nickv\\source\\repos\\Chirpel\\chirpel-react\\src\\pictures\\", "");

            return new UIAccount {
            Id = UserId,
            Username = user.Username,
            Bio = settings.Bio,
            ProfilePicture = pfp,
            IsPrivate = settings.IsPrivate,
            Followers = followers,
            Following = following
            };    
        }

        public DBUser FindUser(string value, string column)
        {
            return _databaseQuery.SelectFirst<DBUser>("User", $"{column}= @Value1", new string[] { value });
        }

        public bool VerifyUser(DBUser user)
        {
            if (_databaseQuery.SelectFirst<DBUser>("User", $"Username= @Value1", new string[] { user.Username} ).Password == user.Password)
                return true;

            return false;
        }

        public ApiResponse AddUser(RegisterUser user)
        {
            Guid id = Guid.NewGuid();

            bool res = _databaseQuery.Insert(new DBUser()
            {
                UserID = id.ToString(),
                Username = user.Username,
                Email = user.Email,
                Password = user.Password
            }, "User");

            if (!res)
                return new ApiResponse(false, "Error inserting into database");

            res = _databaseQuery.Insert(new UserSettings {
            UserId = id,
            DarkModeEnabled = false,
            IsPrivate = false,
            Bio = "",
            ProfilePicture = ""
            }, "User_settings");
            
            if (!res)
                return new ApiResponse(false, "Error inserting into database");

            return new ApiResponse(true, "transaction succesful"); ;
        }

        public ApiResponse DeleteUser(DBUser user)
        {
            DBUser userCheck = _databaseQuery.SelectFirst<DBUser>("User", $"Username= @Value1", new string[] { user.Username });

            if(userCheck!= null && user.Password == userCheck.Password)
            {
                Guid id = Guid.Parse(userCheck.UserID);
                bool res = _databaseQuery.Delete("User_Followers", $"Followed= @Value1 OR Follower= @Value2", new string[] { id.ToString(), id.ToString() });
                if (!res)
                    return new ApiResponse(false, "Error deleting from database");

                res = _databaseQuery.Delete("User_Settings", $"UserID= @Value1", new string[] { id.ToString() });
                if (!res)
                    return new ApiResponse(false, "Error deleting from database");

                res = _databaseQuery.Delete("User", $"UserID= @Value1", new string[] { id.ToString() });
                if (!res)
                    return new ApiResponse(false, "Error deleting from database");

                return new ApiResponse(true, "transaction succesful");
            }

            return new ApiResponse(false, "usercredentials don't match");
        }

        public UserSettings GetSettings(string UserId)
        {
            return _databaseQuery.SelectFirst<UserSettings>("User_settings", $"UserId= @Value1", new string[] { UserId });
        }

        public List<Guid> GetFollowers(string UserId)
        {
            List<Guid> followers = new List<Guid>();
            List<UserFollower> list = _databaseQuery.Select<UserFollower>("User_Followers", $"Followed = @Value1", new string[] { UserId });
            foreach(UserFollower user in list)
                followers.Add(Guid.Parse(user.Follower));

            return followers;
        }

        public ApiResponse AddFollower(string UserId, string FollowerId)
        {
            DBUser user = _databaseQuery.SelectFirst<DBUser>("User", $"UserId= @Value1", new string[] { UserId });

            if (user == null)
                return new ApiResponse(false, "User not found");

            DBUser Follower = _databaseQuery.SelectFirst<DBUser>("User", $"UserId= @Value1", new string[] { FollowerId });
            if (Follower == null)
                return new ApiResponse(false, "Follower not found");

            UserFollower following = _databaseQuery.SelectFirst<UserFollower>("User_followers", $"followed= @Value1 AND follower= @Value2", new string[] { UserId, FollowerId});

            if (following != null)
                return new ApiResponse(false, "already following this user");

            bool res = _databaseQuery.Insert(new UserFollower() {Followed = UserId, Follower = FollowerId }, "User_Followers");

            if (!res)
                return new ApiResponse(false, "Error inserting into database");

            return new ApiResponse(true, "transaction succesful"); 
        }

        public ApiResponse RemoveFollower(string UserId, string FollowerId)
        {
            DBUser user = _databaseQuery.SelectFirst<DBUser>("User", $"UserId= @Value1 ", new string[] { UserId });
            if (user == null)
                return new ApiResponse(false, "User not found");

            DBUser Follower = _databaseQuery.SelectFirst<DBUser>("User", $"UserId= @Value1", new string[] { FollowerId});
            if (Follower == null)
                return new ApiResponse(false, "Follower not found");

            UserFollower following = _databaseQuery.SelectFirst<UserFollower>("User_followers", $"followed= @Value1 AND follower= @Value2", new string[] { UserId, FollowerId });

            if (following == null)
                return new ApiResponse(false, "you aren't following this user");

            bool res = _databaseQuery.Delete("User_followers", $"followed = @Value1 AND follower = @Value2", new string[] { UserId, FollowerId });

            if (!res)
                return new ApiResponse(false, "error deleting from database");

            return new ApiResponse(true, "transaction succesful");
        }

        public List<Guid> GetFollowing(string UserId)
        {
            List<Guid> following = new List<Guid>();
            List<UserFollower> list = _databaseQuery.Select<UserFollower>("User_Followers", $"follower = @Value1", new string[] { UserId });

            foreach (UserFollower user in list)
                following.Add(Guid.Parse(user.Followed));
            
            return following;
        }

        public ApiResponse SetProfilePicture(IFormFile picture, string UserId)
        {
            try
            {
                if (!Directory.Exists("C:\\Users\\nickv\\source\\repos\\Chirpel\\chirpel-react\\src\\pictures"))
                {
                    Directory.CreateDirectory("C:\\Users\\nickv\\source\\repos\\Chirpel\\chirpel-react\\src\\pictures");
                }
                using (FileStream fileStream = File.Create($"C:\\Users\\nickv\\source\\repos\\Chirpel\\chirpel-react\\src\\pictures\\{UserId}.png"))
                {
                    picture.CopyTo(fileStream);
                    fileStream.Flush();
                    bool res = _databaseQuery.Update("User_Settings", "ProfilePicture= @value1", "userId = @value2", new string[] { $"C:\\Users\\nickv\\source\\repos\\Chirpel\\chirpel-react\\src\\pictures\\{UserId}.png", UserId });
                    if (res)
                        return new ApiResponse(true, "succesful");
                    return new ApiResponse(false, "error inserting");
                }
            }
            catch (Exception ex)
            {
                return new ApiResponse(false, ex.Message.ToString());
            }
        }
    }
}
