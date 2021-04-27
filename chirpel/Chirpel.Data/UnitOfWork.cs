using Chirpel.Contract.Interfaces;
using Chirpel.Contract.Interfaces.DAL;
using Chirpel.Data.DAL;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chirpel.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        public IPostDAL Post { get; private set; }

        public IUserDAL User { get; private set; }

        public IUserSettingsDAL UserSettings { get; private set; }

        public IUserFollowersDAL UserFollowers { get; private set; }

        public IPostLikesDAL PostLikes { get; set; }

        public UnitOfWork(PostDAL postDAL, UserDAL userDAL, UserSettingsDAL userSettingsDAL, UserFollowersDAL userFollowersDAL, PostLikesDAL postLikesDAL)
        {
            Post = postDAL;
            User = userDAL;
            UserSettings = userSettingsDAL;
            UserFollowers = userFollowersDAL;
            PostLikes = postLikesDAL;
        }
    }
}
