using Chirpel.Factory;
using Chirpel.Contract.Interfaces.DAL;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chirpel.Logic.User
{
    public class UserCollection
    {
        public List<UserLogic> Users { get; private set; }
        private readonly IUserDAL _userDal;

        public UserCollection()
        {
            Users = new List<UserLogic>();
            _userDal = Factory.Factory.CreateIUserDAL();
        }

        public void GetAll()
        {
            List<Contract.Models.Account.User> users = _userDal.GetAll();

            foreach(Contract.Models.Account.User user in users)
            {
                Users.Add(new UserLogic(user.Username, user.Email, user.Password, user.Id));
            }
        }
    }
}
