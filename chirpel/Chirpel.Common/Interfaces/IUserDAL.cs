﻿using Chirpel.Common.Models.Account;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chirpel.Common.Interfaces
{
    public interface IUserDAL : IDAL<User>
    {
        User GetByUsername(string username);
    }
}
