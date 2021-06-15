using Chirpel.Logic.Account;
using Chirpel.Logic.Message;
using Chirpel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chirpel
{
    public static class UIUtilities
    {
        public static UIUser ConvertToUIUser(UserLogic user)
        {
            user.GetFollowing();
            List<string> following = new List<string>();
            foreach(UserLogic userLogic in user.Following)
            {
                following.Add(userLogic.Id);
            }

            UserCollection userCollection = new UserCollection();
            userCollection.GetFollowers(user.Id);

            List<string> followers = new List<string>();
            foreach (UserLogic userLogic in userCollection.Users)
            {
                followers.Add(userLogic.Id);
            }


            PostCollection postCollection = new PostCollection();
            postCollection.GetAllPostsFromUser(user.Id);

            List<UIPost> posts = ConvertToUIPosts(postCollection.Posts);

            UIUser uIUser = new UIUser()
            {
                Id = user.Id,
                Username = user.Username,
                Bio = user.Bio,
                ProfilePicture = user.ProfilePicture,
                IsPrivate = user.IsPrivate,
                Following = following,
                Followers = followers,
                Posts = posts
            };
            return uIUser;
        }

        public static UIPost ConvertToUIPost(PostLogic post)
        {
            List<string> likes = new List<string>();

            foreach (UserLogic userLogic in post.Likes)
            {
                likes.Add(userLogic.Id);
            }

            UIPost uIPost = new UIPost()
            {
                PostId = post.Id,
                UserId = post.User.Id,
                Content = post.Content,
                Username = post.User.Username,
                PostDate = post.PostDate,
                Userpfp = post.User.ProfilePicture,
                Likes = likes
            };
            return uIPost;
        }

        public static List<UIUser> ConvertToUIUsers(List<UserLogic> _users)
        {
            List<UIUser> posts = new List<UIUser>();

            foreach (UserLogic user in _users)
            {
                posts.Add(ConvertToUIUser(user));
            }
            return posts;
        }

        public static List<UIPost> ConvertToUIPosts(List<PostLogic> _posts)
        {
            List<UIPost> posts = new List<UIPost>();

            foreach (PostLogic post in _posts)
            {
                posts.Add(ConvertToUIPost(post));
            }
            return posts;
        }
    }
}
