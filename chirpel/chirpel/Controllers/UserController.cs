using Chirpel.Models;
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
        public IEnumerable<User> Get()
        {
            List<User> users = new List<User>();
            users = userManager.GetAllUsers();
            return users;

        }

        [HttpPut]
        public HttpResponse Put()
        {
            return new HttpResponse()
            {
                Succes = true,
                Message = "test"
            };
        }

        [HttpPost("login")]
        public HttpResponse Post(LoginUser loginUser)
        {
            User user = userManager.FindUser(loginUser.Username, "Username");
            if(user != null  && user.Password == loginUser.Password)
            {
                return new HttpResponse()
                {
                    Succes = true,
                    Message = $"login succesful"
                };
            }
            return new HttpResponse()
            {
                Succes = false,
                Message = $"user credentials don't match"
            };
        }

        [HttpPost("Delete")]
        public HttpResponse Post(DeleteUser deleteUser)
        {
            User user = userManager.FindUser(deleteUser.Username, "Username");
            if (user != null && user.Password == deleteUser.Password)
            {
                bool response = userManager.DeleteUser(deleteUser);
                if (response)
                {
                    return new HttpResponse()
                    {
                        Succes = true,
                        Message = $"delete succesful"
                    };
                }
                return new HttpResponse()
                {
                    Succes = false,
                    Message = $"Error deleting user from database"
                };

            }
            return new HttpResponse()
            {
                Succes = false,
                Message = $"user credentials don't match"
            };
        }

        [HttpPost("Register")]
        public HttpResponse Post(RegisterUser registerUser)
        {
            User user = userManager.FindUser(registerUser.Username, "Username");
            if (user != null)
            {
                return new HttpResponse()
                {
                    Succes = false,
                    Message = $"Username {user.Username} already exists"
                };
            }
            bool response = userManager.AddUser(registerUser);
            if (response)
            {
                return new HttpResponse()
                {
                    Succes = true,
                    Message = $"account created"
                };
            }
            return new HttpResponse()
            {
                Succes = false,
                Message = $"unknown error"
            };

        }
    }
}
