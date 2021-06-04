using Chirpel.Factory;
using Chirpel.Contract.Interfaces.DAL;
using Chirpel.Logic.Post;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chirpel.Logic.User
{
    public class UIPost
    {
        public string PostId { get; private set; }
        public string Content { get; set; }
        public string UserId { get; set; }
        public DateTime PostDate { get; set; }
        public List<string> Likes { get; set; }
        public List<string> Comments { get; set; }
        public string Userpfp { get; set; }
        public string Username { get; set; }

        private readonly IPostDAL _postDAL;

        public UIPost(IPostDAL postDAL)
        {
            _postDAL = postDAL;
        }

        public UIPost()
        {
            _postDAL = Factory.Factory.CreateIPostDAL();
        }

        public UIPost(string id, string content, string userId, DateTime postDate, List<string> likes, List<string> comments, string userpfp, string username)
        {
            PostId = id;
            Content = content;
            UserId = userId;
            PostDate = postDate;
            Likes = likes;
            Comments = comments;
            Userpfp = userpfp;
            Username = username;
        }

        public void GetFromPost(PostLogic postLogic)
        {
            if (postLogic.Id == null)
                return;

            PostId = postLogic.Id;
            Content = postLogic.Content;
            UserId = postLogic.UserId;
            PostDate = postLogic.PostDate;

            PostLikesLogic postLikesLogic = new PostLikesLogic();

            Likes = postLikesLogic.GetLikes(PostId);
            Comments = new List<string>();

            UserLogic user = new UserLogic();
            user.GetById(UserId);

            UIUser uiUser = new UIUser();
            uiUser.GetFromUser(user);

            Userpfp = uiUser.ProfilePicture;
            Username = uiUser.Username;
        }
    }
}
