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
            User user = userManager.FindUser(loginUser.Username,loginUser.Password);
            if(user != null)
            {
                return new HttpResponse()
                {
                    Succes = true,
                    Message = $"{user.Username}, {user.Password}"
                };
            }
            return new HttpResponse()
            {
                Succes = false,
                Message = $"login requirements not met"
            };
        }

        [HttpPost("Register")]
        public HttpResponse Post(RegisterUser registerUser)
        {
            User user = userManager.FindUser(registerUser.Username,registerUser.Password);
            if (user != null)
            {
                return new HttpResponse()
                {
                    Succes = false,
                    Message = $"Username {user.Username} already exists"
                };
            }
            bool response = userManager.CreateUser(registerUser);
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
