using Chirpel.Models;
using Chirpel.Managers;
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
        public IEnumerable<User> GetAll()
        {
            List<User> users = new List<User>();
            users = userManager.GetAllUsers();
            foreach (User user in users)
                user.Password = "";

            return users;
        }

        [HttpGet("{Username}")]
        public IEnumerable<User> GetUser(string Username)
        {
            List<User> users = new List<User>();
            User user = userManager.FindUser(Username, "Username");

            if (user != null)
                user.Password = "";
            users.Add(user);
            return users;
        }


        [HttpPost("login")]
        public HttpResponse PostLogin(DBUser dbUser)
        {
            User user = userManager.FindUser(dbUser.Username, "Username");
            if (user != null && user.Password == dbUser.Password)
                return new HttpResponse(true, $"login succesful");

            return new HttpResponse(false, $"user credentials don't match");
        }

        [HttpPost("Delete")]
        public HttpResponse PostDelete(DBUser dbUser)
        {
            User user = userManager.FindUser(dbUser.Username, "Username");
            if (user != null && user.Password == dbUser.Password)
            {
                bool response = userManager.DeleteUser(dbUser);
                if (response)
                    return new HttpResponse(true, $"delete succesful");

                return new HttpResponse(false, $"Error deleting user from database");
            }
            return new HttpResponse(false, $"user credentials don't match");
        }

        [HttpPost("Register")]
        public HttpResponse PostRegister(RegisterUser registerUser)
        {
            User user = userManager.FindUser(registerUser.Username, "Username");
            if (user != null)
                return new HttpResponse(false, $"Username {user.Username} already exists");

            bool response = userManager.AddUser(registerUser);
            if (response)
                return new HttpResponse(true, $"account created");

            return new HttpResponse(false, $"unknown error");
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
            foreach( Guid guid in followerIds)
                followers.Add(userManager.FindUser(guid.ToString(), "UserId").Username);

            return followers;
        }

        [HttpPost("follow/{UserId}")]
        public HttpResponse AddFollower(DBUser follower, string UserId)
        {
            if (userManager.VerifyUser(follower) && userManager.FindUser(UserId,"UserId") != null)
            {
                bool res = userManager.AddFollower(UserId, follower.Username);
                if (res)
                    return new HttpResponse(true, "Follow succesfull");
                else
                    return new HttpResponse(false, "Already following");
            }
            return new HttpResponse(false, "Couldn't verify user");
        }
    }
}
