using System;
using System.Collections.Generic;
using System.Text;

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
