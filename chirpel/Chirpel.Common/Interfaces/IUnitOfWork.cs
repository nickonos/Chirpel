using System;
using System.Collections.Generic;
using System.Text;
using Chirpel.Contract.Interfaces.DAL;

namespace Chirpel.Contract.Interfaces
{
    public interface IUnitOfWork
    {
        IPostDAL Post { get; }
        IUserDAL User { get; }
        IUserSettingsDAL UserSettings { get; }
        IUserFollowersDAL UserFollowers { get; }
    }
}
