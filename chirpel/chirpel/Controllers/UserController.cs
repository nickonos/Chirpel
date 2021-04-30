﻿using System.Security.Claims;
using Chirpel.Logic;
using Chirpel.Logic.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using Chirpel.Logic.User;
using Chirpel.Contract.Interfaces.Auth;
using Chirpel.Models;

namespace Chirpel.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IAuthService _authService;

        public UserController(JWTService authService)
        {
            _authService = authService;
        }

        [HttpGet]
        public IEnumerable<UserLogic> GetAll()
        {
            //UserLogic userLogic = new UserLogic();
            List<UserLogic> users = new List<UserLogic>();
            return users;
        }

        [HttpGet("{UserId}")]
        public UIUser GetUser(string UserId)
        {
            UserLogic user = new UserLogic();
            user.GetById(UserId);

            UIUser account = new UIUser();
            account.GetFromUser(user);

            return account;
        }


        [HttpPost("VerifyUser")]
        public ApiResponse VerifyUser(VerificationToken token)
        {
            if (!_authService.IsTokenValid(token.Value))
                return new ApiResponse(false, "invalid verification token");

            List<Claim> claims = _authService.GetTokenClaims(token.Value).ToList();
            string userId = claims.FirstOrDefault(e => e.Type.Equals(ClaimTypes.Name)).Value;

            return new ApiResponse(true, userId);
        }

        [HttpPost("login")]
        public ApiResponse PostLogin(LoginUser loginUser)
        {
            UserLogic user = new UserLogic(loginUser.Username, loginUser.Password);
            Response res = user.Login();

            return new ApiResponse(res.Succes, res.Message);
        }

        [HttpPost("Delete")]
        public ApiResponse PostDelete(VerificationToken token)
        {
            if (!_authService.IsTokenValid(token.Value))
                return new ApiResponse(false, "invalid verification token");

            List<Claim> claims = _authService.GetTokenClaims(token.Value).ToList();
            UserLogic user = new UserLogic();
            user.GetById(claims.FirstOrDefault(e => e.Type.Equals(ClaimTypes.Name)).Value);

            Response res = user.Remove();

            return new ApiResponse(res.Succes, res.Message);
        }

        [HttpPost("Register")]
        public ApiResponse PostRegister(RegisterUser registerUser)
        {
            UserLogic user = new UserLogic(registerUser.Username, registerUser.Email, registerUser.Password);
            Response res = user.Register();
            return new ApiResponse(res.Succes, res.Message);
        }

        [HttpPost("settings")]
        public UserSettingsLogic GetSettings(VerificationToken token)
        {
            if (!_authService.IsTokenValid(token.Value))
                return new UserSettingsLogic();

            List<Claim> claims = _authService.GetTokenClaims(token.Value).ToList();
            UserLogic user = new UserLogic();
            user.GetById(claims.FirstOrDefault(e => e.Type.Equals(ClaimTypes.Name)).Value);

            UserSettingsLogic userSettings = new UserSettingsLogic();
            userSettings.GetById(user.Id);
            return userSettings;
        }

        [HttpGet("{UserId}/followers")]
        public IEnumerable<string> GetFollowers(string UserId)
        {
            UserFollowerLogic userFollower = new UserFollowerLogic();
            List<string> followers = userFollower.GetFollowers(UserId);

            return followers;
        }

        [HttpPost("follow/{UserId}")]
        public ApiResponse AddFollower(VerificationToken token, string UserId)
        {
            if (!_authService.IsTokenValid(token.Value))
                return new ApiResponse(false, "Invalid verificationtoken");

            List<Claim> claims = _authService.GetTokenClaims(token.Value).ToList();
            UserLogic user = new UserLogic();
            user.GetById(claims.FirstOrDefault(e => e.Type.Equals(ClaimTypes.Name)).Value);

            UserFollowerLogic userFollower = new UserFollowerLogic(UserId, user.Id);
            userFollower.Add();

            return new ApiResponse(true, "Follower succesfull");
        }

        [HttpPost("unfollow/{UserId}")]
        public ApiResponse RemoveFollower(VerificationToken token, string UserId)
        {
            if (!_authService.IsTokenValid(token.Value))
                return new ApiResponse(false, "Invalid verificationtoken");

            List<Claim> claims = _authService.GetTokenClaims(token.Value).ToList();
            UserLogic user = new UserLogic();
            user.GetById(claims.FirstOrDefault(e => e.Type.Equals(ClaimTypes.Name)).Value);

            UserFollowerLogic userFollower = new UserFollowerLogic(UserId, user.Id);
            userFollower.Remove();

            return new ApiResponse(true, "Follower succesfull");
        }

        [HttpGet("{UserId}/following")]
        public IEnumerable<string> GetFollowing(string UserId)
        {
            UserFollowerLogic userFollower = new UserFollowerLogic();
            List<string> following = userFollower.GetFollowing(UserId);

            return following;
        }

        [HttpPost("settings/ProfilePicture"), DisableRequestSizeLimit]
        public ApiResponse UpdadateProfilepicture([FromForm] ProfilePictureModel profilePictureModel)
        {
            if (!_authService.IsTokenValid(profilePictureModel.Token))
                return new ApiResponse(false, "invalid verificationtoken");

            List<Claim> claims = _authService.GetTokenClaims(profilePictureModel.Token).ToList();

            UserLogic user = new UserLogic();
            user.GetById(claims.FirstOrDefault(e => e.Type.Equals(ClaimTypes.Name)).Value);

            UserSettingsLogic userSettings = new UserSettingsLogic();
            Response res = userSettings.SetProfilePicture(profilePictureModel.Picture, user.Id);

            return new ApiResponse(res.Succes, res.Message);
        }
    }
}
