using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Chirpel.Common.Models;
using Chirpel.Data;

namespace Chirpel.Logic
{
    public class UserManager
    {
        private readonly DatabaseQuery databaseQuery = new DatabaseQuery();

        public List<DBUser> GetAllUsers()
        {
            return databaseQuery.Select<DBUser>("User");
        }

        public DBUser FindUser(string value, string column)
        {
            return databaseQuery.SelectFirst<DBUser>("User", $"{column}='{value}'");
        }

        public bool VerifyUser(DBUser user)
        {
            if (databaseQuery.SelectFirst<DBUser>("User", $"Username='{user.Username}'").Password == user.Password)
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
            DBUser userCheck = databaseQuery.SelectFirst<DBUser>("Username", $"Username='{user.Username}'");

            if(userCheck!= null && user.Password == userCheck.Password)
            {
                Guid id = Guid.Parse(userCheck.UserID);
                bool res = databaseQuery.Delete("User_Followers", $"Followed='{id}' OR Follower='{id}'");
                if (!res)
                    return new HttpResponse(false, "Error deleting from database");

                res = databaseQuery.Delete("User_Settings", $"UserID='{id}'");
                if (!res)
                    return new HttpResponse(false, "Error deleting from database");

                res = databaseQuery.Delete("User", $"UserID='{id}'");
                if (!res)
                    return new HttpResponse(false, "Error deleting from database");

                return new HttpResponse(true, "transaction succesful");
            }

            return new HttpResponse(false, "usercredentials don't match");
        }

        public UserSettings GetSettings(string UserId)
        {
            return databaseQuery.SelectFirst<UserSettings>("User_settings", $"UserId ='{UserId}'");
        }

        public List<Guid> GetFollowers(string UserId)
        {
            List<Guid> followers = new List<Guid>();
            List<UserFollower> list = databaseQuery.Select<UserFollower>("User_Followers", $"Followed = @userId", new SQLinjection[] { new SQLinjection {name = "@userid", value = UserId } } );
            foreach(UserFollower user in list)
                followers.Add(Guid.Parse(user.Follower));

            return followers;
        }

        public HttpResponse AddFollower(string UserId, string FollowerName)
        {
            DBUser user = databaseQuery.SelectFirst<DBUser>("User", $"UserId='{UserId}'");

            if (user == null)
                return new HttpResponse(false, "User not found");

            string FollowerId = databaseQuery.SelectFirst<DBUser>("User", $"Username='{FollowerName}'").UserID;
            if (FollowerId == null)
                return new HttpResponse(false, "Follower not found");

            string test = databaseQuery.SelectFirst<string>("User_followers", $"followed='{UserId}' AND follower='{FollowerId}'");

            if (test != null)
                return new HttpResponse(false, "already following this user");

            bool res = databaseQuery.Insert(new UserFollower() {Followed = UserId, Follower = FollowerId }, "User_Followers");

            if (!res)
                return new HttpResponse(false, "Error inserting into database");

            return new HttpResponse(true, "transaction succesful"); 
        }

        public HttpResponse RemoveFollower(string UserId, string FollowerName)
        {
            DBUser user = databaseQuery.SelectFirst<DBUser>("User", $"UserId='{UserId}'");
            if (user == null)
                return new HttpResponse(false, "User not found");

            string FollowerId = databaseQuery.SelectFirst<DBUser>("User", $"Username='{FollowerName}'").UserID;
            if (FollowerId == null)
                return new HttpResponse(false, "Follower not found");

            bool res = databaseQuery.Delete("User_followers", $"followed = '{UserId}' AND follower = '{FollowerId}'");

            if (!res)
                return new HttpResponse(false, "error deleting from database");

            return new HttpResponse(true, "transaction succesful");
        }

        public List<Guid> GetFollowing(string UserId)
        {
            List<Guid> following = new List<Guid>();
            List<UserFollower> list = databaseQuery.Select<UserFollower>("User_Followers", $"follower = @userid", new SQLinjection[] { new SQLinjection { name = "@userid", value = UserId } });

            foreach (UserFollower user in list)
                following.Add(Guid.Parse(user.Followed));
            
            return following;
        }
    }
}
