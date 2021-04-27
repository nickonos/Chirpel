using Chirpel.Contract.Interfaces;
using Chirpel.Contract.Interfaces.DAL;
using Chirpel.Contract.Models.Account;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chirpel.Data.DAL
{
    public class UserSettingsDAL : DAL<UserSettings>, IUserSettingsDAL
    {
        public UserSettingsDAL(DatabaseQuery databaseQuery) : base(databaseQuery)
        {
        }
    }
}
