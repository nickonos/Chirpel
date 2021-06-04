using Chirpel.Contract.Interfaces.Auth;
using Chirpel.Contract.Interfaces.DAL;
using Chirpel.Logic.Auth;
using System;
using System.Security.Claims;


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
            _userDAL = Factory.Factory.CreateIUserDAL();
            _authService = Factory.Factory.CreateIAuthService();
        }

        public UserLogic(IUserDAL userDAl)
        {
            _userDAL = userDAl;
            _authService = Factory.Factory.CreateIAuthService();
        }

        public UserLogic(IUserDAL userDAl, string username, string email, string password)
        {
            Username = username;
            Email = email;
            Password = password;
            _userDAL = userDAl;
            _authService = Factory.Factory.CreateIAuthService();
        }

        public UserLogic(string username, string email, string password, string id)
        {
            Id = id;
            Username = username;
            Email = email;
            Password = password;
            _userDAL = Factory.Factory.CreateIUserDAL();
            _authService = Factory.Factory.CreateIAuthService();
        }

        public UserLogic(string username, string password)
        {
            Username = username;
            Password = password;
            _userDAL = Factory.Factory.CreateIUserDAL();
            _authService = Factory.Factory.CreateIAuthService();
        }

        public UserLogic(string username, string email, string password)
        {
            Username = username;
            Email = email;
            Password = password;
            _userDAL = Factory.Factory.CreateIUserDAL();
            _authService = Factory.Factory.CreateIAuthService();
        } 

        public void Add()
        {
            if (Id == null)
                Id = Guid.NewGuid().ToString();

            _userDAL.Add(new Contract.Models.Account.User() {Id = Id, Username = Username, Email =Email, Password = Password });
        }

        public Response Remove()
        {
            if (Id == null)
                return new Response(false, "User not found");

            _userDAL.Remove(new Contract.Models.Account.User() { Id = Id });

            return new Response(true, "item removed");
        }

        public void Update()
        {
            if (Id == null)
                return;

            Contract.Models.Account.User user = new Contract.Models.Account.User();
            user.Id = Id;

            if (Username != null)
                user.Username = Username;

            if (Email != null)
                user.Email = Email;

            if (Password != null)
                user.Password = Password;

            _userDAL.Update(user);
        }

        public Response Verify()
        {
            Contract.Models.Account.User user = _userDAL.GetByUsername(Username);

            if (user == null)
                return new Response(false, "username");

            if (user.Password != Password)
                return new Response(false, "password");

            Id = user.Id;

            return new Response(true, "verified user");
        }

        public void GetByUsername(string username)
        {
            if (_userDAL.GetByUsername(username) == null)
                return;

            Contract.Models.Account.User user = _userDAL.GetByUsername(username);
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

        public void GetByPassword(string password)//For test purposes only
        {
            Contract.Models.Account.User user = _userDAL.GetByPassword(password);

            if (user == null)
                return;

            Id = user.Id;
            Username = user.Username;
            Email = user.Email;
            Password = user.Password;
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
            Id = id.ToString();

            Add();

            UserSettingsLogic userSettingsLogic = new UserSettingsLogic(id.ToString());

            userSettingsLogic.Add();

            return new Response(true, "User Registered");
        }
    }
}
