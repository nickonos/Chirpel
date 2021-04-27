using Chirpel.Contract.Interfaces;
using Chirpel.Contract.Interfaces.Auth;
using Chirpel.Contract.Interfaces.DAL;
using Chirpel.Data;
using Chirpel.Data.DAL;
using System;

namespace Chipel.Factory
{
    public static class Factory
    {
        private static DatabaseQuery _databaseQuery = new DatabaseQuery();
        public static IUnitOfWork CreateIUnitofWork()
        {
            DatabaseQuery databaseQuery = new DatabaseQuery();
            return new UnitOfWork(
                new PostDAL(databaseQuery),
                new UserDAL(databaseQuery),
                new UserSettingsDAL(databaseQuery),
                new UserFollowersDAL(databaseQuery),
                new PostLikesDAL(databaseQuery)
                ) ;
        }

        public static IUserDAL CreateIUserDAL()
        {
            return new UserDAL(_databaseQuery);
        }

        public static IAuthService CreateIAuthService()
        {
            return new JWTService(Environment.GetEnvironmentVariable("CHIRPEL_SECRET") ?? "YWJjZGVmZ2hpamtsbW5vcHE=");
        }

        public static IUserFollowersDAL CreateIUserFollowerDAL()
        {
            return new UserFollowersDAL(_databaseQuery);
        }

        public static IUserSettingsDAL CreateIUserSettingsDAL()
        {
            return new UserSettingsDAL(_databaseQuery);
        }
        public static IPostDAL CreateIPostDAL()
        {
            return new PostDAL(_databaseQuery);
        }
        public static IPostLikesDAL CreateIPostLikesDAL()
        {
            return new PostLikesDAL(_databaseQuery);
        }
    }
}
