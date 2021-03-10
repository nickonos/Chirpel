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



namespace Chirpel.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        UserManager userManager = new UserManager();

        private readonly ILogger<UserController> _logger;

        public UserController(ILogger<UserController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<DBUser> GetAll()
        {
            List<DBUser> users = new List<DBUser>();
            users = userManager.GetAllUsers();
            foreach (DBUser user in users)
                user.Password = "";

            return users;
        }

        [HttpGet("{Username}")]
        public IEnumerable<DBUser> GetUser(string Username)
        {
            List<DBUser> users = new List<DBUser>();
            DBUser user = userManager.FindUser(Username, "Username");

            if (user != null)
                user.Password = "";
            users.Add(user);
            return users;
        }


        [HttpPost("login")]
        public HttpResponse PostLogin(DBUser dbUser)
        {
            DBUser user = userManager.FindUser(dbUser.Username, "Username");
            if (user == null)
                return new HttpResponse(false, "username");

            if (user.Password == dbUser.Password)
                return new HttpResponse(true, $"login succesful");
            else
                return new HttpResponse(false, $"password");
        }

        [HttpPost("Delete")]
        public HttpResponse PostDelete(DBUser dbUser)
        {
            DBUser user = userManager.FindUser(dbUser.Username, "Username");
            if (user != null && user.Password == dbUser.Password)
            {
                HttpResponse response = userManager.DeleteUser(dbUser);
                return response;
            }
            return new HttpResponse(false, $"user credentials don't match");
        }

        [HttpPost("Register")]
        public HttpResponse PostRegister(RegisterUser registerUser)
        {
            if (userManager.FindUser(registerUser.Username, "Username") != null)
                return new HttpResponse(false, $"username");

            if (userManager.FindUser(registerUser.Email, "Email") != null)
                return new HttpResponse(false, "email");
            
            return userManager.AddUser(registerUser);
        }

        [HttpGet("{UserId}/settings")]
        public UserSettings GetSettings(string UserId)
        {
            UserSettings settings = userManager.GetSettings(UserId);

            return settings;
        }

        [HttpGet("{UserId}/followers")]
        public IEnumerable<string> GetFollowers(string UserId)
        {
            List<Guid> followerIds = userManager.GetFollowers(UserId);
            List<string> followers = new List<string>();

            foreach(Guid guid in followerIds)
                followers.Add(userManager.FindUser(guid.ToString(), "UserId").Username);

            return followers;
        }

        [HttpPost("follow/{UserId}")]
        public HttpResponse AddFollower(DBUser follower, string UserId)
        {
            if (userManager.VerifyUser(follower) && userManager.FindUser(UserId,"UserId") != null)
            {
                HttpResponse res = userManager.AddFollower(UserId, follower.Username);
                return res;
            }
            return new HttpResponse(false, "Couldn't verify user");
        }

        [HttpPost("unfollow/{UserId}")]
        public HttpResponse RemoveFollower(DBUser follower, string UserId)
        {
            if (userManager.VerifyUser(follower) && userManager.FindUser(UserId, "UserId") != null)
            {
                HttpResponse res = userManager.RemoveFollower(UserId, follower.Username);
                return res;
            }
            return new HttpResponse(false, "Couldn't verify user");
        }

        [HttpGet("{UserId}/following")]
        public IEnumerable<string> GetFollowing(string UserId)
        {
            List<Guid> followerIds = userManager.GetFollowing(UserId);
            List<string> followers = new List<string>();

            foreach (Guid guid in followerIds)
                followers.Add(userManager.FindUser(guid.ToString(), "UserId").Username);

            return followers;
        }

        [HttpPost("{UserId}/settings/bio")]
        public HttpResponse UpdateBio(string UserId)
        {
            return new HttpResponse(false, "eatr");
        }

        [HttpGet("CreateToken/{text}")]
        public HttpResponse CreateToken(string text)
        {
            IAuthContainerModel model = new JWTContainerModel() {
                Claims = new Claim[]
                {
                    new Claim(ClaimTypes.Name, text)
                }
            };
            IAuthService authService = new JWTService(model.SecretKey);

            string token = authService.GenerateToken(model);

            return new HttpResponse(true, token);
        }

        [HttpGet("ValidateToken/{token}")]
        public HttpResponse ValidateToken(string token)
        {
            IAuthService auth = new JWTService(Environment.GetEnvironmentVariable("CHIRPEL_SECRET") ?? "YWJjZGVmZ2hpamtsbW5vcHE=");
            if (!auth.IsTokenValid(token))
                return new HttpResponse(false, "invalid token");

            List<Claim> claims = auth.GetTokenClaims(token).ToList();
                return new HttpResponse(true, claims.FirstOrDefault(e => e.Type.Equals(ClaimTypes.Name)).Value);
        }
    }
}
