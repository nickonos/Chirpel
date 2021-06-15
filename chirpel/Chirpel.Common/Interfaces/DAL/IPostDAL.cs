using Chirpel.Contract.Models.Message;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chirpel.Contract.Interfaces.DAL
{
    public interface IPostDAL : IDAL<Post>
    {
        List<Post> GetPostsOrderbyDesc();
        List<Post> GetByUserId(string userId);
    }
}
