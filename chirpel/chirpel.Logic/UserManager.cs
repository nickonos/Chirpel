using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Chirpel.Common.Models;
using Chirpel.Data;

namespace Chirpel.Logic
{
    public class UserManager
    {
        private readonly string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;Initial Catalog=Chirpel;Integrated Security=True"; //Moeder

        //private readonly string connectionString = "Server=localhost;Database=Chirpel;Trusted_Connection=True;"; // Vader

        DatabaseQuery databaseQuery = new DatabaseQuery();

        public List<DBUser> GetAllUsers()
        {
            List<DBUser> users = new List<DBUser>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand query = new SqlCommand("SELECT * from [User]",conn))
                {
                    conn.Open();

                    var reader = query.ExecuteReader();
                    while (reader.Read())
                    {
                        DBUser temp = new DBUser()
                        {
                            UserID = reader.GetString(0),
                            Username = reader.GetString(1),
                            Email = reader.GetString(2),
                            Password = reader.GetString(3)
                        }; 
                        users.Add(temp);
                    }
                }
            }
            return users;
        }

        public DBUser? FindUser(string value, string Table)
        {
            List<DBUser> users = databaseQuery.Select<DBUser>("User", $"{Table}='{value}'");
            if (users.Count > 0)
                return users[0];
            return null;
        }

        public DBUser FindUserTest()
        {
            List<DBUser> user = databaseQuery.Select<DBUser>("User", $"Username='nick'");
            return user[0];
        }

        public bool VerifyUser(DBUser user)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand query = new SqlCommand($"SELECT * from [User] WHERE Username='{user.Username}'", conn))
                {
                    conn.Open();

                    var reader = query.ExecuteReader();
                    while (reader.Read())
                    {
                        if (reader.GetString(1) == user.Username && reader.GetString(3) == user.Password)
                            return true;
                    }
                }
            }
            return false;
        }

        public HttpResponse AddUser(RegisterUser user)
        {
            Guid id = Guid.NewGuid();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand query = new SqlCommand($"INSERT INTO [User] (UserId, Username, Email, Password) VALUES (@id,@Username,@Email,@Password)", conn))
                {
                    query.Parameters.AddWithValue("@id", id);
                    query.Parameters.AddWithValue("@Username", user.Username);
                    query.Parameters.AddWithValue("@Email", user.Email);
                    query.Parameters.AddWithValue("@Password", user.Password);
                    conn.Open();
                    int result = query.ExecuteNonQuery();

                    if (result < 0)
                        return new HttpResponse(false, "error inserting into database"); ;
                }
                
                using (SqlCommand query = new SqlCommand($"INSERT INTO [User_Settings] (UserId, DarkModeEnabled, IsPrivate, Bio, ProfilePicture) VALUES (@UserId, @DarkModeEnabled, @IsPrivate, @Bio, @ProfilePicture)", conn))
                {
                    query.Parameters.AddWithValue("@UserId", id);
                    query.Parameters.AddWithValue("@DarkModeEnabled", false);
                    query.Parameters.AddWithValue("@IsPrivate", false);
                    query.Parameters.AddWithValue("@Bio", "");
                    query.Parameters.AddWithValue("@ProfilePicture", "");
                    
                    int result = query.ExecuteNonQuery();

                    if (result < 0)
                        return new HttpResponse(false, "error inserting into database"); ;
                }
            }
            return new HttpResponse(true, "transaction succesful"); ;
        }

        public HttpResponse DeleteUser(DBUser user)
        {
            DBUser userCheck = FindUser(user.Username, "Username");
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

        public UserSettings? GetSettings(string UserId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand query = new SqlCommand($"SELECT * from [User_Settings] WHERE UserId ='{UserId}'", conn))
                {
                    conn.Open();
                    var Reader = query.ExecuteReader();
                    while (Reader.Read())
                    {
                        UserSettings settings = new UserSettings
                        {
                            UserId = Guid.Parse(Reader.GetString(0)),
                            DarkModeEnabled = Reader.GetBoolean(1),
                            IsPrivate = Reader.GetBoolean(2),
                            Bio = Reader.GetString(3),
                            ProfilePicture = Reader.GetString(4)
                        };
                        return settings;
                    }
                }
            }
            return null;
        }

        public List<Guid> GetFollowers(string UserId)
        {
            List<Guid> followers = new List<Guid>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand query = new SqlCommand($"SELECT * from [User_Followers] WHERE Followed ='{UserId}'", conn))
                {
                    conn.Open();
                    var Reader = query.ExecuteReader();
                    while (Reader.Read())
                    {
                        followers.Add(Guid.Parse(Reader.GetString(1)));
                    }
                }
            }
            return followers;
        }

        public HttpResponse AddFollower(string UserId, string FollowerName)
        {
            string FollowerId = FindUser(FollowerName, "Username").UserID;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                Guid test = new Guid();
                using (SqlCommand query = new SqlCommand($"SELECT * FROM [User_followers] WHERE followed='{UserId}' AND follower='{FollowerId}'", conn))
                {
                    
                    conn.Open();
                    var Reader = query.ExecuteReader();
                    while (Reader.Read())
                    {
                        test = Guid.Parse(Reader.GetString(0));
                    }
                    Reader.Close();
                }
                if (test != null)
                {
                    using (SqlCommand query = new SqlCommand($"INSERT INTO [User_Followers] (Followed, Follower) VALUES (@followed, @follower)", conn))
                    {
                    query.Parameters.AddWithValue("followed", UserId);
                    query.Parameters.AddWithValue("follower", FollowerId);
                    
                    int result = query.ExecuteNonQuery();

                    if (result < 0)
                        return new HttpResponse(false, "error inserting into database");
                    }
                    return new HttpResponse(true, "transaction succesful");
                }     
            }
            return new HttpResponse(false, "userpair not found"); ;
        }

        public HttpResponse RemoveFollower(string UserId, string FollowerName)
        {
            string FollowerId = FindUser(FollowerName, "Username").UserID;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand query = new SqlCommand($"DELETE FROM [User_followers] WHERE followed='{UserId}' AND follower='{FollowerId}'", conn))
                {
                    conn.Open();

                    int result = query.ExecuteNonQuery();

                    if (result < 0)
                        return new HttpResponse(false, "error deleting from database");
                }
                return new HttpResponse(true, "transaction succesful");
            }
        }

        public List<Guid> GetFollowing(string UserId)
        {
            List<Guid> following = new List<Guid>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand query = new SqlCommand($"SELECT * FROM [User_followers] WHERE follower='{UserId}'", conn))
                {
                    conn.Open();
                    var Reader = query.ExecuteReader();
                    while (Reader.Read())
                    {
                        following.Add(Guid.Parse(Reader.GetString(0)));
                    }
                }
            }
            return following;
        }
    }
}
