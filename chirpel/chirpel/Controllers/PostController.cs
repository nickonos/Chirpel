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
using System.Threading.Tasks;


namespace Chirpel.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IAuthService _authService;
        public PostController(IAuthService authservice)
        {
            _authService = authservice;
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

        //[HttpGet("explore")]
        //public List<UIPost> GetExplore()
        //{
        //    return postManager.GetExploreFeed();
        //}
        //[HttpGet("explore/{lastPost}")]
        //public List<UIPost> GetExplore(string lastPost)
        //{
        //    return postManager.GetExploreFeed(lastPost);
        //}

        //[HttpPost("personal")]
        //public List<UIPost> GetPersonal(VerificationToken token)
        //{
        //    if (!_authService.IsTokenValid(token.Value))
        //        return new List<UIPost>();

        //    return postManager.GetPersonalFeed(token.Value);
        //}

        //[HttpPost("personal/{lastPost}")]
        //public List<UIPost> GetPersonal(VerificationToken token, string lastPost)
        //{
        //    if (!_authService.IsTokenValid(token.Value))
        //        return new List<UIPost>();

        //    return postManager.GetPersonalFeed(token.Value, lastPost);
        //}

        //[HttpPost("create")]
        //public ApiResponse Post(NewPost newPost)
        //{
        //    if (!_authService.IsTokenValid(newPost.Token))
        //        return new ApiResponse(false, "invalid token");

        //    PostLogic post = new PostLogic();
        //    post.
        //    return postManager.CreatePost(newPost);
        //}
    }
}
