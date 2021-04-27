﻿using Chipel.Factory;
using Chirpel.Contract.Interfaces.Auth;
using Chirpel.Contract.Interfaces.DAL;
using Chirpel.Contract.Models.Account;
using System;
using System.Collections.Generic;
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
            _userSettingsDAL = Factory.CreateIUserSettingsDAL();
            _authService = Factory.CreateIAuthService();
        }

        public UserSettingsLogic(string id, bool darkmodeEnabled, bool isPrivate, string bio, string profilePicture)
        {
            _userSettingsDAL = Factory.CreateIUserSettingsDAL();
            _authService = Factory.CreateIAuthService();

            Id = id;
            DarkModeEnabled = darkmodeEnabled;
            IsPrivate = IsPrivate;
            Bio = bio;
            ProfilePicture = profilePicture;
        }

        public void Add()
        {

        }

        public void GetById(string id)
        {
            UserSettings userSettings = _userSettingsDAL.Get(id);

            Id = userSettings.Id;
            DarkModeEnabled = userSettings.DarkModeEnabled;
            IsPrivate = userSettings.IsPrivate;
            Bio = userSettings.Bio;
            ProfilePicture = userSettings.ProfilePicture;
        }
    }
}
