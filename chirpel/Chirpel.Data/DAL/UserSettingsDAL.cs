using Chirpel.Common.Interfaces;
using Chirpel.Common.Models.Account;
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
