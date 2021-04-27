using Chipel.Factory;
using Chirpel.Contract.Interfaces.Auth;
using Chirpel.Contract.Interfaces.DAL;
using Chirpel.Logic.Auth;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using Chirpel.Contract.Models.Account;

namespace Chirpel.Logic.User
{
    public class UserLogic
    {
        public string Id { get; private set; }
        public string Username { get; private set; }
        public string Email { get; private set; }
        public string Password { get; private set; }

        private readonly IUserDAL _userDAL;
        private readonly IAuthService _authService;

        public UserLogic()
        {
            _userDAL = Factory.CreateIUserDAL();
            _authService = Factory.CreateIAuthService();
        }

        public UserLogic(string username, string email, string password, string id)
        {
            Id = id;
            Username = username;
            Email = email;
            Password = password;
        }

        public UserLogic(string username, string password)
        {
            Username = username;
            Password = password;
        }

        public UserLogic(string username, string email, string password)
        {
            Username = username;
            Email = email;
            Password = password;
        } 

        public void Add()
        {
            _userDAL.Add(new Contract.Models.Account.User() {Id = Id, Username = Username, Email =Email, Password = Password });
        }

        public Response Remove()
        {
            return new Response(false, "not implemented");
        }

        public void Update()
        {

        }

        public Response Verify()
        {
            Contract.Models.Account.User user = _userDAL.GetByUsername(Username);

            if (user == null)
                return new Response(false, "username");

            if (user.Password != Password)
                return new Response(false, "password");

            return new Response(true, "verified user");
        }

        public void GetByUsername(string username)
        {
            Contract.Models.Account.User user = _userDAL.GetByUsername(username);

            if (user == null)
                return;

            Id = user.Id;
            Username = user.Username;
            Email = user.Email;
            Password = user.Password;
        }

        public void GetByEmail(string email)
        {
            Contract.Models.Account.User user = _userDAL.GetByEmail(email);

            if (user == null)
                return;

            Id = user.Id;
            Username = user.Username;
            Email = user.Email;
            Password = user.Password;
        }

        public void GetById(string id)
        {
            Contract.Models.Account.User user = _userDAL.Get(id);

            if (user == null)
                return;

            Id = user.Id;
            Username = user.Username;
            Email = user.Email;
            Password = user.Password;
        }

        public Response Login()
        {
            GetByUsername(Username);

            Response res = Verify();

            if (!res.Succes)
                return res;

            IAuthContainerModel model = new JWTContainerModel()
            {
                Claims = new Claim[]
                {
                    new Claim(ClaimTypes.Name, Id)
                }
            };
            string token = _authService.GenerateToken(model);

            return new Response(true, token);
        }


        public Response Register()
        {
            GetByUsername(Username);
            if (Id != null)
                return new Response(false, "username");

            GetByEmail(Email);
            if (Id != null)
                return new Response(false, "email");
            Guid id = Guid.NewGuid();
            UserLogic user = new UserLogic(Username, Email, Password, id.ToString());

            user.Add();

            UserSettingsLogic userSettingsLogic = new UserSettingsLogic(id.ToString(), false, false, "", "");

            userSettingsLogic.Add();

            return new Response(true, "User Registered");
        }
    }
}
