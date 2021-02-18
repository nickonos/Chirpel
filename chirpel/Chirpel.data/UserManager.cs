using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Chirpel.data
{
    public class UserManager
    {
        //public string connectionString = "Data Source=(LocalDB)/MSSQLLocalDB;AttachDbFilename=C:/Users/nickv/source/repos/Chirpel/chirpel/Chirpel.data/Chirpel.mdf;Integrated Security=True";
        private string connectionString = "Server=studmysql01.fhict.local; Database=dbi434661; Uid=dbi434661; Pwd=daivbot;";
        public List<User> GetAllUsers()
        {
            List<User> users = new List<User>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand query = new SqlCommand("select * from User"))
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
