using Chirpel.Common.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Chirpel.Logic
{
    public class PostManager
    {
        private readonly string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;Initial Catalog=Chirpel;Integrated Security=True;";

        public Post GetPost(string id)
        {
            Post post = new Post();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand query = new SqlCommand($"SELECT * from [Post] WHERE PostId='{id}'", conn))
                {
                    conn.Open();

                    var reader = query.ExecuteReader();
                    while (reader.Read())
                    {
                        post.id = Guid.Parse(reader.GetString(0));
                        post.Content = reader.GetString(1);
                        post.UserId = Guid.Parse(reader.GetString(2));
                        post.PostTime = reader.GetDateTime(3);
                    }
                }
            }
            return post;
        }

        public List<Post> GetFriendsPost(string UserId)
        {
            return new List<Post>();
        }
    }
}
