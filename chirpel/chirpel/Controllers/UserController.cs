using Chirpel.Common.Models;
using Chirpel.Common.Models.Auth;
using System.Security.Claims;
using Chirpel.Logic;
using Chirpel.Logic.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Chirpel.Common.Models.Account;
using Microsoft.AspNetCore.Http;
using System.IO;
using Chirpel.Data;

namespace Chirpel.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;

        private readonly IAuthService _authService;

        private readonly UserManager _userManager; 

        public UserController(ILogger<UserController> logger, JWTService authService, DatabaseQuery databaseQuery)
        {
            _logger = logger;
            _authService = authService;
            _userManager = new UserManager(authService, databaseQuery);
        }

        [HttpGet]
        public IEnumerable<User> GetAll()
        {
            //List<User> users = _userManager.GetAllUsers();
            _userManager.TestMethod();
            List<User> users = new List<User>();

            foreach (User user in users)
                user.Password = "";

            return users;
        }

        [HttpGet("{UserId}")]
        public UIAccount GetUser(string UserId)
        {
            UIAccount account =_userManager.GetUIAccount(UserId);
            return account;
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
            return _userManager.Login(loginUser);
        }

        [HttpPost("Delete")]
        public ApiResponse PostDelete(VerificationToken token)
        {
            if (!_authService.IsTokenValid(token.Value))
                return new ApiResponse(false, "invalid verification token");

            List<Claim> claims = _authService.GetTokenClaims(token.Value).ToList();
            User user = _userManager.FindUserById(claims.FirstOrDefault(e => e.Type.Equals(ClaimTypes.Name)).Value);
             
            return _userManager.DeleteUser(user);
        }

        [HttpPost("Register")]
        public ApiResponse PostRegister(RegisterUser registerUser)
        {
            if (_userManager.FindUserByName(registerUser.Username) != null)
                return new ApiResponse(false, $"username");

            if (_userManager.FindUserByName(registerUser.Email) != null)
                return new ApiResponse(false, "email");
            
            return _userManager.AddUser(registerUser);
        }

        [HttpPost("settings")]
        public UserSettings GetSettings(VerificationToken token)
        {
            if (!_authService.IsTokenValid(token.Value))
                return new UserSettings();

            List<Claim> claims = _authService.GetTokenClaims(token.Value).ToList();
            User user = _userManager.FindUserById(claims.FirstOrDefault(e => e.Type.Equals(ClaimTypes.Name)).Value);

            return _userManager.GetSettings(user.Id);
        }

        [HttpGet("{UserId}/followers")]
        public IEnumerable<string> GetFollowers(string UserId)
        {
            List<Guid> followerIds = _userManager.GetFollowers(UserId);
            List<string> followers = new List<string>();

            foreach(Guid guid in followerIds)
                followers.Add(_userManager.FindUserById(guid.ToString()).Username);

            return followers;
        }

        [HttpPost("follow/{UserId}")]
        public ApiResponse AddFollower(VerificationToken token, string UserId)
        {
            if (!_authService.IsTokenValid(token.Value))
                return new ApiResponse(false, "invalid verificationtoken");

            List<Claim> claims = _authService.GetTokenClaims(token.Value).ToList();
            User user = _userManager.FindUserById(claims.FirstOrDefault(e => e.Type.Equals(ClaimTypes.Name)).Value);

            return _userManager.AddFollower(UserId, user.Id);
        }

        [HttpPost("unfollow/{UserId}")]
        public ApiResponse RemoveFollower(VerificationToken token, string UserId)
        {
            if (!_authService.IsTokenValid(token.Value))
                return new ApiResponse(false, "invalid verificationtoken");

            List<Claim> claims = _authService.GetTokenClaims(token.Value).ToList();
            User user = _userManager.FindUserById(claims.FirstOrDefault(e => e.Type.Equals(ClaimTypes.Name)).Value);
  
            return _userManager.RemoveFollower(UserId, user.Id);
        }

        [HttpGet("{UserId}/following")]
        public IEnumerable<string> GetFollowing(string UserId)
        {
            List<Guid> followerIds = _userManager.GetFollowing(UserId);
            List<string> followers = new List<string>();

            foreach (Guid guid in followerIds)
                followers.Add(_userManager.FindUserById(guid.ToString()).Username);

            return followers;
        }

        [HttpPost("settings/ProfilePicture"), DisableRequestSizeLimit]
        public ApiResponse UpdadateProfilepicture([FromForm] ProfilePictureModel profilePictureModel)
        {
            if (!_authService.IsTokenValid(profilePictureModel.token))
                return new ApiResponse(false, "invalid verificationtoken");

            List<Claim> claims = _authService.GetTokenClaims(profilePictureModel.token).ToList();

            User user = _userManager.FindUserById(claims.FirstOrDefault(e => e.Type.Equals(ClaimTypes.Name)).Value);
            
            return _userManager.SetProfilePicture(profilePictureModel.picture, user.Id);
        }
    }
}
