using Chirpel.Common.Models;
using Chirpel.Common.Models.Auth;
using Chirpel.Common.Models.Post;
using Chirpel.Data;
using Chirpel.Logic;
using Chirpel.Logic.Auth;
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
        private readonly PostManager postManager;

        private readonly IAuthService _authService;
        public PostController(JWTService authservice, DatabaseQuery databaseQuery)
        {
            postManager = new PostManager(authservice, databaseQuery);
            _authService = authservice;
        }

        [HttpGet("{id}")]
        public Post Get(string id)
        {
            return postManager.GetPost(id);
        }

        [HttpGet("explore")]
        public List<UIPost> GetExplore()
        {
            return postManager.GetExploreFeed();
        }
        [HttpGet("explore/{lastPost}")]
        public List<UIPost> GetExplore(string lastPost)
        {
            return postManager.GetExploreFeed(lastPost);
        }

        [HttpPost("personal")]
        public List<UIPost> GetPersonal(VerificationToken token)
        {
            if (!_authService.IsTokenValid(token.Value))
                return new List<UIPost>();

            return postManager.GetPersonalFeed(token.Value);
        }

        [HttpPost("personal/{lastPost}")]
        public List<UIPost> GetPersonal(VerificationToken token, string lastPost)
        {
            if (!_authService.IsTokenValid(token.Value))
                return new List<UIPost>();

            return postManager.GetPersonalFeed(token.Value, lastPost);
        }

        [HttpPost("create")]
        public ApiResponse Post(NewPost newPost)
        {
            if (!_authService.IsTokenValid(newPost.Token))
                return new ApiResponse(false, "invalid token");

            return postManager.CreatePost(newPost);
        }

        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
