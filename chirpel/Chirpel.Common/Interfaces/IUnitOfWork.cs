using System;
using System.Collections.Generic;
using System.Text;
using Chirpel.Common.Interfaces.DAL;

namespace Chirpel.Common.Interfaces
{
    public interface IUnitOfWork
    {
        IPostDAL Post { get; }
        IUserDAL User { get; }
        IUserSettingsDAL UserSettings { get; }
        IUserFollowersDAL UserFollowers { get; }
    }
}
