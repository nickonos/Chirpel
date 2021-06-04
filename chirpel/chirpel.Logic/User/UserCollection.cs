using Chirpel.Contract.Interfaces.DAL;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chirpel.Logic.User
{
    public class UserCollection
    {
        public List<UserLogic> Users { get; private set; }

        private readonly IUserDAL _userDAL;

        public UserCollection()
        {
            _userDAL = Factory.Factory.CreateIUserDAL();
            Users = new List<UserLogic>();
        }

        public UserCollection(IUserDAL userDAL)
        {
            _userDAL = userDAL;
            Users = new List<UserLogic>();
        }

        public void GetAll()
        {
            List<Contract.Models.Account.User> users = _userDAL.GetAll();

            foreach(Contract.Models.Account.User user in users)
            {
                Users.Add(new UserLogic(user.Username, user.Email, user.Password, user.Id));
            }
        }

    }
}
