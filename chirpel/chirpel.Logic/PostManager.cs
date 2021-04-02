using Chirpel.Common.Models;
using Chirpel.Common.Models.Account;
using Chirpel.Common.Models.Post;
using Chirpel.Data;
using Chirpel.Logic.Auth;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Chirpel.Logic
{
    public class PostManager
    {
        private readonly DatabaseQuery _databaseQuery;
        private readonly IAuthService _authService;
        private readonly UserManager _userManager;
        public PostManager(DatabaseQuery databaseQuery, JWTService authService)
        {
            _databaseQuery = databaseQuery;
            _authService = authService;
            _userManager = new UserManager(authService, databaseQuery);
        }
      
        public DBPost GetPost(string id)
        {
            return _databaseQuery.SelectFirst<DBPost>("Post","postid=@value1",new string[] { id});
        }

        public List<DBPost> GetAllPost()
        {
            return _databaseQuery.Select<DBPost>("Post","PostDate DESC");
        }

        public List<UIPost> GetExploreFeed()
        {
            List<DBPost> posts = _databaseQuery.Select<DBPost>("Post", "PostDate DESC");
            List<UIPost> feed = new List<UIPost>();

            foreach(DBPost post in posts)
            {
                if (feed.Count >= 10)
                    return feed;

                UIAccount user = _userManager.GetUIAccount(post.UserId);
                if (!user.IsPrivate)
                {
                    feed.Add(new UIPost()
                    {
                        PostId = post.PostId,
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
            List<DBPost> posts = _databaseQuery.Select<DBPost>("Post", "PostDate DESC");
            List<UIPost> feed = new List<UIPost>();

            if (posts.Find(x => x.PostId == postId) != null)
            {
                int i = posts.IndexOf(posts.Find(x => x.PostId == postId));
                posts.RemoveRange(0, i+1);
            }

            foreach (DBPost post in posts)
            {
                if (feed.Count >= 10)
                    return feed;

                UIAccount user = _userManager.GetUIAccount(post.UserId);
                if (!user.IsPrivate)
                {
                    feed.Add(new UIPost()
                    {
                        PostId = post.PostId,
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

            List<DBPost> posts = _databaseQuery.Select<DBPost>("Post", "PostDate DESC");
            List<UIPost> feed = new List<UIPost>();
            List<Guid> following = _userManager.GetFollowing(claims.FirstOrDefault(e => e.Type.Equals(ClaimTypes.Name)).Value);

            foreach (DBPost post in posts)
            {
                if (feed.Count >= 10)
                    return feed;

                UIAccount user = _userManager.GetUIAccount(post.UserId);
                if (following.Contains(Guid.Parse(post.UserId)))
                {
                    feed.Add(new UIPost()
                    {
                        PostId = post.PostId,
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

            List<DBPost> posts = _databaseQuery.Select<DBPost>("Post", "PostDate DESC");
            List<UIPost> feed = new List<UIPost>();
            List<Guid> following = _userManager.GetFollowing(claims.FirstOrDefault(e => e.Type.Equals(ClaimTypes.Name)).Value);

            if(posts.Find(x => x.PostId == postId) != null)
            {
                int i = posts.IndexOf(posts.Find(x => x.PostId == postId));
                posts.RemoveRange(0, i);
            }

            foreach (DBPost post in posts)
            {
                if (feed.Count >= 10)
                    return feed;

                UIAccount user = _userManager.GetUIAccount(post.UserId);
                if (following.Contains(Guid.Parse(post.UserId)))
                {
                    feed.Add(new UIPost()
                    {
                        PostId = post.PostId,
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
            DBPost post = new DBPost()
            {
                PostId = Guid.NewGuid().ToString(),
                Content = newPost.Content,
                UserId = claims.FirstOrDefault(e => e.Type.Equals(ClaimTypes.Name)).Value,
                PostDate = DateTime.UtcNow
            };
            bool res = _databaseQuery.Insert<DBPost>(post, "Post");
            if(res)
                return new ApiResponse(true, "transaction succesful");

            return new ApiResponse(false, "error inserting into database");
        }
    }
}
