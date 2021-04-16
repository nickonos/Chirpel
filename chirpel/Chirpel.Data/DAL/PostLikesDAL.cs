﻿using Chirpel.Common.Interfaces;
using Chirpel.Common.Interfaces.DAL;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chirpel.Data.DAL
{
    public class PostLikesDAL : DAL<PostLikesDAL>, IPostLikesDAL
    {
        public PostLikesDAL(DatabaseQuery databaseQuery) : base(databaseQuery)
        {
        }
    }
}