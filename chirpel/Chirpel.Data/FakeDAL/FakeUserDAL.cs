using Chirpel.Contract.Interfaces;
using Chirpel.Contract.Interfaces.DAL;
using Chirpel.Contract.Models.Account;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chirpel.Data.FakeDAL
{
    public class FakeUserDAL : IUserDAL
    {   
        public List<User> Users { get; private set; }

        public User GetByUsername(string username)
        {
            return Users.Find(c => c.Username == username);
        }

        public User GetByEmail(string email)
        {
            return Users.Find(c => c.Email == email);
        }

        public User GetByPassword(string password)
        {
            return Users.Find(c => c.Password == password);
        }

        public User Get(string id)
        {
            return Users.Find(c => c.Id == id);
        }

        public List<User> GetAll()
        {
            return Users;
        }

        public void Add(User entity)
        {
            Users.Add(entity);
        }

        public void Remove(User entity)
        {
            Users.Remove(entity);
        }

        public void Update(User entity)
        {
            User user = Users.Find(c => c.Id == entity.Id);

            if (entity.Username != null)
                user.Username = entity.Username;

            if (entity.Email != null)
                user.Email = entity.Email;

            if (entity.Password != null)
                user.Password = entity.Password;
        }

        public FakeUserDAL()
        {
            Users = new List<User>();
        }
    }
}
