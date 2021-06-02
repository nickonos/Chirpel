using Chirpel.Contract.Models.Account;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chirpel.Contract.Interfaces.DAL
{
    public interface IUserDAL : IDAL<User>
    {
        User GetByUsername(string username);
        User GetByEmail(string email);
        User GetByPassword(string password);
    }
}
