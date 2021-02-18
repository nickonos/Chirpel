using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Chirpel.data
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
    }
}
