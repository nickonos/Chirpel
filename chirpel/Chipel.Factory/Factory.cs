using Chirpel.Common.Interfaces;
using Chirpel.Data;
using Chirpel.Data.DAL;
using System;

namespace Chipel.Factory
{
    public static class Factory
    {
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
    }
}
