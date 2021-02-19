using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Chirpel.Models;

namespace Chirpel.Controllers
{
    public class UserManager
    {
        public string connectionString = "Data Source = (LocalDB)\\MSSQLLocalDB;;Integrated Security=True;AttachDbFilename=C:\\Users\\nickv\\source\\repos\\Chirpel\\chirpel\\Chirpel.data\\Chirpel.mdf;";

        public List<User> GetAllUsers()
        {
            List<User> users = new List<User>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand query = new SqlCommand("select * from [User]",conn))
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
                using (SqlCommand query = new SqlCommand($"select * from [User] Where {Table}='{value}'", conn))
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
            }
            return true;
        }

        public bool DeleteUser(DeleteUser user)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand query = new SqlCommand($"DELETE FROM [User] WHERE Username='{user.Username}' AND Password='{user.Password}'", conn))
                {

                    conn.Open();
                    int result = query.ExecuteNonQuery();


                    if (result < 0)
                        return false;
                }
            }
            return true;
        }
    }
}
