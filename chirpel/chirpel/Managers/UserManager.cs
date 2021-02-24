using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Chirpel.Models;

namespace Chirpel.Managers
{
    public class UserManager
    {
        private readonly string connectionString = "Data Source = (LocalDB)\\MSSQLLocalDB; Integrated Security=True;AttachDbFilename=C:\\Users\\nickv\\source\\repos\\Chirpel\\chirpel\\Chirpel.data\\Chirpel.mdf;";

        public List<User> GetAllUsers()
        {
            List<User> users = new List<User>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand query = new SqlCommand("SELECT * from [User]",conn))
                {
                    conn.Open();

                    var reader = query.ExecuteReader();
                    while (reader.Read())
                    {
                        User temp = new User();
                        temp.id = reader.GetString(0);
                        temp.Username = reader.GetString(1);
                        temp.Email = reader.GetString(2);
                        temp.Password = reader.GetString(3);
                        users.Add(temp);
                    }
                }
            }
            return users;
        }

        public User? FindUser(string value, string Table)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand query = new SqlCommand($"SELECT * from [User] Where {Table}='{value}'", conn))
                {
                    conn.Open();

                    var reader = query.ExecuteReader();
                    while (reader.Read())
                    {
                        User temp = new User()
                        {
                        id = reader.GetString(0),
                        Username = reader.GetString(1),
                        Email = reader.GetString(2),
                        Password = reader.GetString(3)
                        };
                        return temp;
                    }
                }
            }
            return null;
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

        public bool AddUser(RegisterUser user)
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
                        return false;
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
                        return false;
                }
            }
            return true;
        }

        public bool DeleteUser(DBUser user)
        {
            Guid id = Guid.Parse(FindUser(user.Username, "Username").id);
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand query = new SqlCommand($"DELETE FROM [User_Followers] WHERE Followed='{id}' OR Follower='{id}'",conn))
                {
                    conn.Open();

                    int result = query.ExecuteNonQuery();

                    if (result < 0)
                        return false;
                }
                using (SqlCommand query = new SqlCommand($"DELETE FROM [User_Settings] WHERE UserID='{id}'", conn))
                {
                    
                    int result = query.ExecuteNonQuery();

                    if (result < 0)
                        return false;
                }
                using (SqlCommand query = new SqlCommand($"DELETE FROM [User] WHERE Username='{user.Username}' AND Password='{user.Password}'", conn))
                {
                    int result = query.ExecuteNonQuery();

                    if (result < 0)
                        return false;
                }
            }
            return true;
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
            string FollowerId = FindUser(FollowerName, "Username").id;
            if (UserId == FollowerId)
                return new HttpResponse(false, "you can't follow yourself");
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string test ="";
                using (SqlCommand query = new SqlCommand($"select * from [User_followers] WHERE followed='{UserId}' AND follower='{FollowerId}'", conn))
                {
                    
                    conn.Open();
                    var Reader = query.ExecuteReader();
                    while (Reader.Read())
                    {
                        test = Reader.GetString(0);
                    }
                    Reader.Close();
                }
                if (test == "")
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
            return new HttpResponse(false, "you already follow this user");
        }

        public HttpResponse RemoveFollower(string UserId, string FollowerName)
        {
            string FollowerId = FindUser(FollowerName, "Username").id;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand query = new SqlCommand($"DELETE FROM [User_followers] WHERE followed='{UserId}' AND follower='{FollowerId}'", conn))
                {

                    conn.Open();
                    int response = query.ExecuteNonQuery();

                    if (response < 0)
                        return new HttpResponse(false, "Error inserting into database");

                    return new HttpResponse(true, "transaction succesful");
                }
            }
        }

        public List<Guid> GetFollowing(string UserId)
        {
            List<Guid> followers = new List<Guid>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand query = new SqlCommand($"SELECT * from [User_Followers] WHERE Follower ='{UserId}'", conn))
                {
                    conn.Open();
                    var Reader = query.ExecuteReader();
                    while (Reader.Read())
                    {
                        followers.Add(Guid.Parse(Reader.GetString(0)));
                    }
                }
            }
            return followers;
        }
    }
}
