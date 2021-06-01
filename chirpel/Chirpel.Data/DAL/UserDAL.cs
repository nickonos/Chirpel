using Chirpel.Contract.Interfaces;
using Chirpel.Contract.Interfaces.DAL;
using Chirpel.Contract.Models.Account;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chirpel.Data.DAL
{
    public class UserDAL : DAL<User>, IUserDAL
    {   
        public User GetByUsername(string username)
        {
            return _databaseQuery.SelectFirst<User>($"Username= @Value1", new string[] { username });
        }

        public User GetByEmail(string email)
        {
            return _databaseQuery.SelectFirst<User>($"Email= @Value1", new string[] { email });
        }

        public UserDAL(DatabaseQuery databaseQuery) : base(databaseQuery)
        {
        }


    }
}
