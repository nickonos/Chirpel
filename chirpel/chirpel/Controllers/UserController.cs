using System.Security.Claims;
using Chirpel.Logic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using Chirpel.Logic.Account;
using Chirpel.Contract.Interfaces.Auth;
using Chirpel.Models;
using Chirpel.Factory;
using Microsoft.AspNetCore.Cors;
using Chirpel.Logic.Message;

namespace Chirpel.Controllers
{
    [EnableCors("venus")]
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IAuthService _authService;

        public UserController()
        {
            _authService = Factory.Factory.CreateIAuthService();
        }

        [HttpGet]
        public IEnumerable<UserLogic> GetAll()
        {
            UserCollection userCollection = new UserCollection();
            userCollection.GetAll();
            return userCollection.Users;
        }

        [HttpGet("{UserId}")]
        public UIUser GetUser(string UserId)
        {
            UserLogic user = new UserLogic();
            user.GetById(UserId);

            UIUser uIUser = UIUtilities.ConvertToUIUser(user);
            return uIUser;
        }


        [HttpPost("VerifyUser")]
        public ApiResponse VerifyUser(VerificationToken token)
        {
            if (!_authService.IsTokenValid(token.Value))
                return new ApiResponse(false, "invalid verification token");

            List<Claim> claims = _authService.GetTokenClaims(token.Value).ToList();
            string userId = claims.FirstOrDefault(e => e.Type.Equals(ClaimTypes.Name)).Value;

            return new ApiResponse(true, userId);
        }

        [HttpPost("login")]
        public ApiResponse PostLogin(LoginUser loginUser)
        {
            UserLogic user = new UserLogic(loginUser.Username, loginUser.Password);
            Response res = user.Login();

            return new ApiResponse(res.Succes, res.Message);
        }

        [HttpPost("Delete")]
        public ApiResponse PostDelete(VerificationToken token)
        {
            if (!_authService.IsTokenValid(token.Value))
                return new ApiResponse(false, "invalid verification token");

            List<Claim> claims = _authService.GetTokenClaims(token.Value).ToList();
            string id = claims.FirstOrDefault(e => e.Type.Equals(ClaimTypes.Name)).Value;

            UserCollection userCollection = new UserCollection();
            Response res = userCollection.RemoveUser(id);

            return new ApiResponse(res.Succes, res.Message);
        }

        [HttpPost("Register")]
        public ApiResponse PostRegister(RegisterUser registerUser)
        {

            UserCollection userCollection = new UserCollection();
            Response res = userCollection.Register(registerUser.Username, registerUser.Email, registerUser.Password);
           
            return new ApiResponse(res.Succes, res.Message);
        }

        [HttpPost("settings/bio")]
        public ApiResponse UpdateBio(UpdateBio updateBio)
        {
            if (!_authService.IsTokenValid(updateBio.Token))
                return new ApiResponse(false, "invalid token");

            List<Claim> claims = _authService.GetTokenClaims(updateBio.Token).ToList();
            string id = claims.FirstOrDefault(e => e.Type.Equals(ClaimTypes.Name)).Value;

            UserLogic userLogic = new UserLogic();
            userLogic.GetById(id);
            userLogic.UpdateBio(updateBio.Bio);

            return new ApiResponse(true, "succes");
        }


        [HttpGet("{UserId}/followers")]
        public IEnumerable<string> GetFollowers(string UserId)
        {
            UserCollection userCollection = new UserCollection();
            userCollection.GetFollowers(UserId);

            List<string> followers = new List<string>();
            foreach(UserLogic userLogic in userCollection.Users)
            {
                followers.Add(userLogic.Id);
            }

            return followers;
        }

        [HttpPost("follow/{UserId}")]
        public ApiResponse AddFollower(VerificationToken token, string UserId)
        {
            if (!_authService.IsTokenValid(token.Value))
                return new ApiResponse(false, "Invalid verificationtoken");

            List<Claim> claims = _authService.GetTokenClaims(token.Value).ToList();
            UserLogic user = new UserLogic();
            user.GetById(claims.FirstOrDefault(e => e.Type.Equals(ClaimTypes.Name)).Value);

            UserLogic userLogic = new UserLogic();
            userLogic.GetById(UserId);

            Response res = user.FollowUser(userLogic);

            return new ApiResponse(res.Succes, res.Message);
        }

        [HttpPost("unfollow/{UserId}")]
        public ApiResponse RemoveFollower(VerificationToken token, string UserId)
        {
            if (!_authService.IsTokenValid(token.Value))
                return new ApiResponse(false, "Invalid verificationtoken");

            List<Claim> claims = _authService.GetTokenClaims(token.Value).ToList();
            UserLogic user = new UserLogic();
            user.GetById(claims.FirstOrDefault(e => e.Type.Equals(ClaimTypes.Name)).Value);

            UserLogic userLogic = new UserLogic();
            userLogic.GetById(UserId);

            Response res = user.UnfollowUser(userLogic);

            return new ApiResponse(res.Succes, res.Message);
        }

        [HttpGet("{UserId}/following")]
        public IEnumerable<string> GetFollowing(string UserId)
        {
            UserLogic userLogic = new UserLogic();
            userLogic.GetById(UserId);
            userLogic.GetFollowing();

            List<string> following = new List<string>();
            foreach (UserLogic userLogic1 in userLogic.Following)
            {
                following.Add(userLogic1.Id);
            }

            return following;
        }
    }
}
