﻿using System;
using System.Collections.Generic;
using System.Text;
using Chirpel.Contract.Interfaces;
using Chirpel.Contract.Interfaces.DAL;
using Chirpel.Contract.Models.Message;

namespace Chirpel.Data.DAL
{
    public class PostDAL : DAL<Post>, IPostDAL
    {
        public List<Post> GetPostsOrderbyDesc()
        {
            return _databaseQuery.Select<Post>("PostDate DESC");
        }

        public List<Post> GetByUserId(string userId)
        {
            return _databaseQuery.Select<Post>("UserId =@value1", new string[] {userId });
        }

        public PostDAL(DatabaseQuery databaseQuery) : base(databaseQuery)
        {
        }
    }
}
