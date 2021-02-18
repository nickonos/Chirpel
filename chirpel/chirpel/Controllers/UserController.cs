using Chirpel.data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace chirpel.Controllers
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
        public IEnumerable<User> Put()
        {
            List<User> users = new List<User>();
            users = userManager.GetAllUsers();
            return users;
        }

        [HttpPost("User/login")]
        public IEnumerable<User> Post()
        {
            List<User> users = new List<User>();
            users = userManager.GetAllUsers();
            return users;
        }
    }
}
