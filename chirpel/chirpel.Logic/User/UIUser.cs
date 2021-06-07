﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using Chirpel.Contract.Models.Post;
using Chirpel.Logic.Post;

namespace Chirpel.Logic.User
{
    public class UIUser
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string Bio { get; set; }
        public string ProfilePicture { get; set; }
        public bool IsPrivate { get; set; }
        public List<string> Followers { get; set; }
        public List<string> Following { get; set; }
        public List<UIPost> Posts { get; set; }

        public UIUser() {
            Followers = new List<string>();
            Following = new List<string>();
            Posts = new List<UIPost>();
        }

        public UIUser(string id, string username, string bio, string profilePicture, bool isPrivate, List<string> followers, List<string> following, List<UIPost> posts)
        {
            Id = id;
            Username = username;
            Bio = bio;
            ProfilePicture = profilePicture;
            IsPrivate = isPrivate;
            Followers = followers;
            Following = following;
            Posts = posts;
        }

        public void GetFromUser(UserLogic user, bool deeper)
        {
            Id = user.Id;
            Username = user.Username;
            
            UserSettingsLogic userSettingsLogic = new UserSettingsLogic();
            userSettingsLogic.GetById(Id);

            Bio = userSettingsLogic.Bio;
            ProfilePicture = userSettingsLogic.ProfilePicture;
            IsPrivate = userSettingsLogic.IsPrivate;

            UserFollowerLogic userFollowerLogic = new UserFollowerLogic();
            Followers = userFollowerLogic.GetFollowers(Id);
            Following = userFollowerLogic.GetFollowing(Id);

            PostCollection postCollection= new PostCollection();
            postCollection.GetAllPostsFromUser(Id);
            if (deeper)
            {
                foreach(PostLogic post in postCollection.Posts)
                {
                    UIPost uIPost = new UIPost();
                    uIPost.GetFromPost(post);
                    Posts.Add(uIPost);
                }
            }
            
        }
    }
}
