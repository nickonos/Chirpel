using Chirpel.Contract.Interfaces.Auth;
using Chirpel.Contract.Models.Message;
using Chirpel.Data;
using Chirpel.Logic;
using Chirpel.Logic.Message;
using Chirpel.Logic.Account;
using Chirpel.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
            UIPost uIPost = UIUtilities.ConvertToUIPost(post);

            return uIPost;
        }

        [HttpGet("explore")]
        public List<UIPost> GetExplore()
        {
            PostCollection postCollection = new PostCollection();
            postCollection.GetExplore();

            return UIUtilities.ConvertToUIPosts(postCollection.Posts);
        }
        [HttpGet("explore/{lastPost}")]
        public List<UIPost> GetExplore(string lastPost)
        {
            PostCollection postCollection = new PostCollection();
            postCollection.GetExplore();

            return UIUtilities.ConvertToUIPosts(postCollection.Posts);
        }

        [HttpPost("personal")]
        public List<UIPost> GetPersonal(VerificationToken token)
        {
            if (!_authService.IsTokenValid(token.Value))
                return new List<UIPost>();

            List<Claim> claims = _authService.GetTokenClaims(token.Value).ToList();
            PostCollection postCollection = new PostCollection();
            postCollection.GetPersonal(claims.FirstOrDefault(e => e.Type.Equals(ClaimTypes.Name)).Value);

            return UIUtilities.ConvertToUIPosts(postCollection.Posts);
        }

        [HttpPost("personal/{lastPost}")]
        public List<UIPost> GetPersonal(VerificationToken token, string lastPost)
        {
            if (!_authService.IsTokenValid(token.Value))
                return new List<UIPost>();

            List<Claim> claims = _authService.GetTokenClaims(token.Value).ToList();
            PostCollection postCollection = new PostCollection();
            postCollection.GetPersonal(claims.FirstOrDefault(e => e.Type.Equals(ClaimTypes.Name)).Value);

            return UIUtilities.ConvertToUIPosts(postCollection.Posts);
        }

        [HttpPost("create")]
        public ApiResponse Post(NewPost newPost)
        {
            if (!_authService.IsTokenValid(newPost.Token))
                return new ApiResponse(false, "Invalid token");

            List<Claim> claims = _authService.GetTokenClaims(newPost.Token).ToList();

            UserLogic user = new UserLogic();
            user.GetById(claims.FirstOrDefault(e => e.Type.Equals(ClaimTypes.Name)).Value);
            PostCollection postCollection = new PostCollection();
            Response response = postCollection.AddPost(newPost.Content, user);
            
            return new ApiResponse(response.Succes, response.Message);
        }
    }
}
