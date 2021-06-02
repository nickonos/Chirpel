using Chirpel.Contract.Interfaces.Auth;
using Chirpel.Contract.Models.Post;
using Chirpel.Data;
using Chirpel.Logic;
using Chirpel.Logic.Auth;
using Chirpel.Logic.Post;
using Chirpel.Logic.User;
using Chirpel.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Chirpel.Factory;
using Microsoft.AspNetCore.Cors;

namespace Chirpel.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [EnableCors("venus")]
    public class PostController : ControllerBase
    {
        private readonly IAuthService _authService;
        public PostController()
        {
            _authService = Factory.Factory.CreateIAuthService() ;
        }

        [HttpGet("{id}")]
        public UIPost Get(string id)
        {
            PostLogic post = new PostLogic();
            post.GetById(id);
            UIPost uiPost = new UIPost();
            uiPost.GetFromPost(post);
            return uiPost;
        }

        [HttpGet("explore")]
        public List<UIPost> GetExplore()
        {
            PostCollection postCollection = new PostCollection();
            postCollection.GetExplore();
            List<UIPost> uIPosts = new List<UIPost>();
            foreach (PostLogic post in postCollection.Posts)
            {
                UIPost uIPost = new UIPost();
                uIPost.GetFromPost(post);
                uIPosts.Add(uIPost);
            }

            return uIPosts;
        }
        [HttpGet("explore/{lastPost}")]
        public List<UIPost> GetExplore(string lastPost)
        {
            PostCollection postCollection = new PostCollection();
            postCollection.GetExplore();
            List<UIPost> uIPosts = new List<UIPost>();
            foreach(PostLogic post in postCollection.Posts)
            {
                UIPost uIPost = new UIPost();
                uIPost.GetFromPost(post);
                uIPosts.Add(uIPost);
            }

            return uIPosts;
        }

        [HttpPost("personal")]
        public List<UIPost> GetPersonal(VerificationToken token)
        {
            if (!_authService.IsTokenValid(token.Value))
                return new List<UIPost>();

            List<Claim> claims = _authService.GetTokenClaims(token.Value).ToList();
            PostCollection postCollection = new PostCollection();
            postCollection.GetPersonal(claims.FirstOrDefault(e => e.Type.Equals(ClaimTypes.Name)).Value);

            List<UIPost> uIPosts = new List<UIPost>();
            foreach (PostLogic post in postCollection.Posts)
            {
                UIPost uIPost = new UIPost();
                uIPost.GetFromPost(post);
                uIPosts.Add(uIPost);
            }

            return uIPosts;
        }

        [HttpPost("personal/{lastPost}")]
        public List<UIPost> GetPersonal(VerificationToken token, string lastPost)
        {
            if (!_authService.IsTokenValid(token.Value))
                return new List<UIPost>();

            List<Claim> claims = _authService.GetTokenClaims(token.Value).ToList();
            PostCollection postCollection = new PostCollection();
            postCollection.GetPersonal(claims.FirstOrDefault(e => e.Type.Equals(ClaimTypes.Name)).Value);

            List<UIPost> uIPosts = new List<UIPost>();
            foreach (PostLogic post in postCollection.Posts)
            {
                UIPost uIPost = new UIPost();
                uIPost.GetFromPost(post);
                uIPosts.Add(uIPost);
            }

            return uIPosts;
        }

        [HttpPost("create")]
        public ApiResponse Post(NewPost newPost)
        {
            if (!_authService.IsTokenValid(newPost.Token))
                return new ApiResponse(false, "Invalid token");

            List<Claim> claims = _authService.GetTokenClaims(newPost.Token).ToList();

            PostLogic post = new PostLogic(newPost.Content, claims.FirstOrDefault(e => e.Type.Equals(ClaimTypes.Name)).Value);
            post.Add();
            return new ApiResponse(true, post.Id);
        }
    }
}
