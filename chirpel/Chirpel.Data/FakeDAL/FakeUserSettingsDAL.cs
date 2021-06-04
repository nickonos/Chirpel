using Chirpel.Contract.Interfaces;
using Chirpel.Contract.Interfaces.DAL;
using Chirpel.Contract.Models.Account;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chirpel.Data.FakeDAL
{
    public class FakeUserSettingsDAL :  IUserSettingsDAL
    {
        public List<UserSettings> UserSettings { get; private set; }

        public void Add(UserSettings entity)
        {
            UserSettings.Add(entity);
        }

        public UserSettings Get(string id)
        {
            return UserSettings.Find(c => c.Id == id);
        }

        public List<UserSettings> GetAll()
        {
            return UserSettings;
        }

        public void Remove(UserSettings entity)
        {
            UserSettings.Remove(entity);
        }

        public void Update(UserSettings entity)
        {
            UserSettings userSettings = UserSettings.Find(c => c.Id == entity.Id);
            if (entity.IsPrivate != null)
                userSettings.IsPrivate = entity.IsPrivate;

            if (entity.ProfilePicture != null)
                userSettings.ProfilePicture = entity.ProfilePicture;

            if (entity.Bio != null)
                userSettings.Bio = entity.Bio;

            if (entity.DarkModeEnabled != null)
                userSettings.DarkModeEnabled = entity.DarkModeEnabled;
        }

        public FakeUserSettingsDAL()
        {
            UserSettings = new List<UserSettings>();
        }
    }
}
