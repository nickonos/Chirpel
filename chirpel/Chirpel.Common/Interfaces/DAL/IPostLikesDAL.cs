using Chirpel.Contract.Models.Message;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chirpel.Contract.Interfaces.DAL
{
    public interface IPostLikesDAL : IDAL<PostLikes>
    {
        List<PostLikes> GetPostLikes(string postId);
        List<PostLikes> GetPostLikesFromUser(string userId);
        PostLikes GetPostLikes(string postId, string UserId);
    }
}
