using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Security.Claims;
using Chipel.Factory;
using Chirpel.Common.Interfaces;
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
        private readonly IAuthService _authService;
        private readonly IUnitOfWork _unitOfWork;
        public UserManager(IAuthService authService, DatabaseQuery databaseQuery)
        {
            _authService = authService;
            _unitOfWork = Factory.CreateIUnitofWork(databaseQuery);
        }

        public List<User> GetAllUsers()
        {
            return _unitOfWork.User.GetAll().ToList();
        }

        public ApiResponse Login(LoginUser user)
        {
            User dbUser = _unitOfWork.User.GetByUsername(user.Username);

            if (dbUser == null)
                return new ApiResponse(false, "username");

            if (dbUser.Password == user.Password)
            {
                IAuthContainerModel model = new JWTContainerModel()
                {
                    Claims = new Claim[]
                {
                    new Claim(ClaimTypes.Name, dbUser.Id)
                }
                };
                string token = _authService.GenerateToken(model);
                return new ApiResponse(true, token);
            }

            return new ApiResponse(false, "password");
        }

        public UIAccount GetUIAccount(string UserId)
        {
            User user = _unitOfWork.User.Get(UserId);
            if (user == null)
                return new UIAccount();

            UserSettings settings = _unitOfWork.UserSettings.Get(UserId);

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

        public User FindUserByName(string name)
        {
            return _unitOfWork.User.GetByUsername(name);
        }

        public User FindUserById(string id)
        {
            return _unitOfWork.User.Get(id);
        }

        public ApiResponse AddUser(RegisterUser user)
        {
            Guid id = Guid.NewGuid();

            _unitOfWork.User.Add(new User()
            {
                Id = id.ToString(),
                Username = user.Username,
                Email = user.Email,
                Password = user.Password
            });

            _unitOfWork.UserSettings.Add(new UserSettings
            {
                Id = id,
                DarkModeEnabled = false,
                IsPrivate = false,
                Bio = "",
                ProfilePicture = ""
            });

            return new ApiResponse(true, "transaction succesful"); ;
        }

        public ApiResponse DeleteUser(User user)
        {
            User userCheck = _unitOfWork.User.GetByUsername(user.Username);

            if(userCheck!= null && user.Password == userCheck.Password)
            {
                Guid id = Guid.Parse(userCheck.Id);
                _unitOfWork.UserFollowers.Remove(new UserFollower() { Followed = id.ToString() });
                //bool res = _databaseQuery.Delete("User_Followers", $"Followed= @Value1 OR Follower= @Value2", new string[] { id.ToString(), id.ToString() });
                //if (!res)
                //    return new ApiResponse(false, "Error deleting from database");

                //res = _databaseQuery.Delete("User_Settings", $"Id= @Value1", new string[] { id.ToString() });
                //if (!res)
                //    return new ApiResponse(false, "Error deleting from database");

                //res = _databaseQuery.Delete("User", $"Id= @Value1", new string[] { id.ToString() });
                //if (!res)
                //    return new ApiResponse(false, "Error deleting from database");

                return new ApiResponse(true, "transaction succesful");
            }

            return new ApiResponse(false, "usercredentials don't match");
        }

        public UserSettings GetSettings(string UserId)
        {
            return _unitOfWork.UserSettings.Get(UserId);
        }

        public List<Guid> GetFollowers(string UserId)
        {
            List<Guid> followers = new List<Guid>();
            List<UserFollower> list = _unitOfWork.UserFollowers.GetAll().ToList();
            foreach(UserFollower user in list)
                followers.Add(Guid.Parse(user.Follower));

            return followers;
        }

        public ApiResponse AddFollower(string UserId, string FollowerId)
        {
            User user = _unitOfWork.User.Get(UserId);

            if (user == null)
                return new ApiResponse(false, "User not found");

            User Follower = _unitOfWork.User.Get(FollowerId);
            if (Follower == null)
                return new ApiResponse(false, "Follower not found");

            UserFollower following = _unitOfWork.UserFollowers.GetFollowers(UserId, FollowerId);

            if (following != null)
                return new ApiResponse(false, "already following this user");

            _unitOfWork.UserFollowers.Add(new UserFollower() { Followed = UserId, Follower = FollowerId });

            return new ApiResponse(true, "transaction succesful"); 
        }

        public ApiResponse RemoveFollower(string UserId, string FollowerId)
        {
            User user = _unitOfWork.User.Get(UserId);
            if (user == null)
                return new ApiResponse(false, "User not found");

            User Follower = _unitOfWork.User.Get(FollowerId);
            if (Follower == null)
                return new ApiResponse(false, "Follower not found");

            UserFollower following = _unitOfWork.UserFollowers.GetFollowers(UserId, FollowerId);

            if (following == null)
                return new ApiResponse(false, "you aren't following this user");

            _unitOfWork.UserFollowers.Remove(new UserFollower() { Followed = UserId, Follower = FollowerId });

            return new ApiResponse(true, "transaction succesful");
        }

        public List<Guid> GetFollowing(string UserId)
        {
            List<Guid> following = new List<Guid>();
            List<UserFollower> list = _unitOfWork.UserFollowers.GetAll().ToList();//TODO implement Get where with 1 variable
            

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
                    _unitOfWork.UserSettings.Update(new UserSettings() {ProfilePicture = $"C:\\Users\\nickv\\source\\repos\\Chirpel\\chirpel-react\\src\\pictures\\{UserId}.png" });
                    
                    return new ApiResponse(true, "succesful");
                }
            }
            catch (Exception ex)
            {
                return new ApiResponse(false, ex.Message.ToString());
            }
        }
        public void TestMethod()
        {
            _unitOfWork.User.Remove(new User() { Username = "test", Email = "test@mail", Password = "testpass", Id = Guid.NewGuid().ToString() });
        }
    }
}
