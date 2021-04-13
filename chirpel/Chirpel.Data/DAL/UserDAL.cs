using Chirpel.Common.Interfaces;
using Chirpel.Common.Models.Account;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chirpel.Data.DAL
{
    public class UserDAL : DAL<User>, IUserDAL
    {
        public UserDAL(DatabaseQuery databaseQuery) : base(databaseQuery)
        {
        }

        public User GetByUsername(string username)
        {
            return _databaseQuery.SelectFirst<User>("User", $"Username= @Value1", new string[] { username });
        }
    }
}
