using Chipel.Factory;
using Chirpel.Common.Interfaces;
using Chirpel.Common.Interfaces.Auth;
using Chirpel.Common.Models;
using Chirpel.Common.Models.Account;
using Chirpel.Common.Models.Post;
using Chirpel.Logic.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Chirpel.Logic
{
    public class PostManager
    {
        private readonly IAuthService _authService;
        private readonly UserManager _userManager;
        private readonly IUnitOfWork _unitofWork;
        public PostManager(JWTService authService)
        {
            _authService = authService;
            _userManager = new UserManager(authService);
            _unitofWork = Factory.CreateIUnitofWork();
        }
      
        public Post GetPost(string id)
        {
            return _unitofWork.Post.Get(id);
        }

        public List<Post> GetAllPost()
        {
            return _unitofWork.Post.GetPostsOrderbyDesc();
        }

        public List<UIPost> GetExploreFeed()
        {
            List<Post> posts = _unitofWork.Post.GetPostsOrderbyDesc();
            List<UIPost> feed = new List<UIPost>();

            foreach (Post post in posts)
            {
                if (feed.Count >= 10)
                    return feed;

                UIAccount user = _userManager.GetUIAccount(post.UserId);
                if (!user.IsPrivate)
                {
                    feed.Add(new UIPost()
                    {
                        PostId = post.Id,
                        Content = post.Content,
                        PostDate = post.PostDate,
                        UserId = post.UserId,
                        Username = user.Username,
                        Userpfp = user.ProfilePicture
                    });
                }
            }

            return feed;
        }

        public List<UIPost> GetExploreFeed(string postId)
        {
            List<Post> posts = _unitofWork.Post.GetPostsOrderbyDesc();
            List<UIPost> feed = new List<UIPost>();

            if (posts.Find(x => x.Id == postId) != null)
            {
                int i = posts.IndexOf(posts.Find(x => x.Id == postId));
                posts.RemoveRange(0, i+1);
            }

            foreach (Post post in posts)
            {
                if (feed.Count >= 10)
                    return feed;

                UIAccount user = _userManager.GetUIAccount(post.UserId);
                if (!user.IsPrivate)
                {
                    feed.Add(new UIPost()
                    {
                        PostId = post.Id,
                        Content = post.Content,
                        PostDate = post.PostDate,
                        UserId = post.UserId,
                        Username = user.Username,
                        Userpfp = user.ProfilePicture
                    });
                }
            }

            return feed;
        }

        public List<UIPost> GetPersonalFeed(string token)
        {
            List<Claim> claims = _authService.GetTokenClaims(token).ToList();

            List<Post> posts = _unitofWork.Post.GetPostsOrderbyDesc();
            List<UIPost> feed = new List<UIPost>();
            List<Guid> following = _userManager.GetFollowing(claims.FirstOrDefault(e => e.Type.Equals(ClaimTypes.Name)).Value);

            foreach (Post post in posts)
            {
                if (feed.Count >= 10)
                    return feed;

                UIAccount user = _userManager.GetUIAccount(post.UserId);
                if (following.Contains(Guid.Parse(post.UserId)))
                {
                    feed.Add(new UIPost()
                    {
                        PostId = post.Id,
                        Content = post.Content,
                        PostDate = post.PostDate,
                        UserId = post.UserId,
                        Username = user.Username,
                        Userpfp = user.ProfilePicture
                    });
                }
            }

            return feed;
        }

        public List<UIPost> GetPersonalFeed(string token, string postId)
        {
            List<Claim> claims = _authService.GetTokenClaims(token).ToList();

            List<Post> posts = _unitofWork.Post.GetPostsOrderbyDesc();
            List<UIPost> feed = new List<UIPost>();
            List<Guid> following = _userManager.GetFollowing(claims.FirstOrDefault(e => e.Type.Equals(ClaimTypes.Name)).Value);

            if(posts.Find(x => x.Id == postId) != null)
            {
                int i = posts.IndexOf(posts.Find(x => x.Id == postId));
                posts.RemoveRange(0, i);
            }

            foreach (Post post in posts)
            {
                if (feed.Count >= 10)
                    return feed;

                UIAccount user = _userManager.GetUIAccount(post.UserId);
                if (following.Contains(Guid.Parse(post.UserId)))
                {
                    feed.Add(new UIPost()
                    {
                        PostId = post.Id,
                        Content = post.Content,
                        PostDate = post.PostDate,
                        UserId = post.UserId,
                        Username = user.Username,
                        Userpfp = user.ProfilePicture
                    });
                }
            }

            return feed;
        }

        public ApiResponse CreatePost(NewPost newPost)
        {
            List<Claim> claims = _authService.GetTokenClaims(newPost.Token).ToList();
            Post post = new Post()
            {
                Id = Guid.NewGuid().ToString(),
                Content = newPost.Content,
                UserId = claims.FirstOrDefault(e => e.Type.Equals(ClaimTypes.Name)).Value,
                PostDate = DateTime.UtcNow
            };
            bool res = _unitofWork.Post.CreatePost(post);
            if(res)
                return new ApiResponse(true, "transaction succesful");

            return new ApiResponse(false, "error inserting into database");
        }
    }
}
