using Chirpel.Factory;
using Chirpel.Contract.Interfaces.Auth;
using Chirpel.Contract.Interfaces.DAL;
using Chirpel.Contract.Models.Account;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Chirpel.Logic.User
{
    public class UserSettingsLogic
    {
        public string Id { get; private set; }
        public bool DarkModeEnabled { get; private set; }
        public bool IsPrivate { get; private set; }
        public string Bio { get; private set; }
        public string ProfilePicture { get; private set; }

        private readonly IUserSettingsDAL _userSettingsDAL;
        private readonly IAuthService _authService;

        public UserSettingsLogic()
        {
            _userSettingsDAL = Factory.Factory.CreateIUserSettingsDAL();
            _authService = Factory.Factory.CreateIAuthService();
        }

        public UserSettingsLogic(IUserSettingsDAL userSettingsDAL)
        {
            _userSettingsDAL = userSettingsDAL;
            _authService = Factory.Factory.CreateIAuthService();
        }

        public UserSettingsLogic(IUserSettingsDAL userSettingsDAL, string id)
        {
            Id = id;
            _userSettingsDAL = userSettingsDAL;
            _authService = Factory.Factory.CreateIAuthService();
        }

        public UserSettingsLogic(string id)
        {
            _userSettingsDAL = Factory.Factory.CreateIUserSettingsDAL();
            _authService = Factory.Factory.CreateIAuthService();

            Id = id;
        }

        public UserSettingsLogic(string id, string bio)
        {
            _userSettingsDAL = Factory.Factory.CreateIUserSettingsDAL();
            _authService = Factory.Factory.CreateIAuthService();

            Id = id;
            Bio = bio;
        }

        public UserSettingsLogic(string id, bool darkmodeEnabled, bool isPrivate, string bio, string profilePicture)
        {
            _userSettingsDAL = Factory.Factory.CreateIUserSettingsDAL();
            _authService = Factory.Factory.CreateIAuthService();

            Id = id;
            DarkModeEnabled = darkmodeEnabled;
            IsPrivate = isPrivate;
            Bio = bio;
            ProfilePicture = profilePicture;
        }

        public void Add()
        {
            if (Id == null)
                return;

            if (DarkModeEnabled == null)
                DarkModeEnabled = false;

            if (IsPrivate == null)
                IsPrivate = false;

            if (Bio == null)
                Bio = "";

            if (ProfilePicture == null)
                ProfilePicture = "";

            _userSettingsDAL.Add(new UserSettings() { Id = Id, ProfilePicture = ProfilePicture, Bio = Bio, DarkModeEnabled = DarkModeEnabled, IsPrivate = IsPrivate });
        }

        public Response Remove()
        {
            if (Id == null)
                return new Response(false, "UserSettings not found");

            _userSettingsDAL.Remove(new UserSettings() { Id = Id });

            return new Response(true, "item removed");
        }

        public bool GetById(string id)
        {
            if (id == null)
                return false;
            UserSettings userSettings = _userSettingsDAL.Get(id);
            if (userSettings == null)
                return false;

            Id = userSettings.Id;
            DarkModeEnabled = userSettings.DarkModeEnabled;
            IsPrivate = userSettings.IsPrivate;
            Bio = userSettings.Bio;
            ProfilePicture = userSettings.ProfilePicture;
            return true;
        }

        public void Update()
        {
            if (Id == null)
                return;
            UserSettings user = new UserSettings() { Id = Id };
            if (Bio != null)
                user.Bio = Bio;
            if (DarkModeEnabled != null)
                user.DarkModeEnabled = DarkModeEnabled;
            if (IsPrivate != null)
                user.IsPrivate = IsPrivate;
            if (ProfilePicture != null)
                user.ProfilePicture = ProfilePicture;

            _userSettingsDAL.Update(user);
        }

        public Response SetProfilePicture(IFormFile picture, string UserId)
        {
            try
            {
                if (!Directory.Exists("C:\\Users\\nickv\\source\\repos\\Chirpel\\chirpel-react\\src\\pictures"))
                {
                    Directory.CreateDirectory("C:\\Users\\nickv\\source\\repos\\Chirpel\\chirpel-react\\src\\pictures");
                }
                using (FileStream fileStream = File.Create($"C:\\Users\\nickv\\source\\repos\\Chirpel\\chirpel-react\\src\\pictures\\{UserId}.png"))
                {
                    picture.CopyTo(fileStream);
                    fileStream.Flush();
                    _userSettingsDAL.Update(new UserSettings() { ProfilePicture = $"C:\\Users\\nickv\\source\\repos\\Chirpel\\chirpel-react\\src\\pictures\\{UserId}.png" });

                    return new Response(true, "succesful");
                }
            }
            catch (Exception ex)
            {
                return new Response(false, ex.Message.ToString());
            }
        }
    }
}
