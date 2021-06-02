using Chirpel.Controllers;
using Chirpel.Logic.User;
using Chirpel.Models;
using NUnit.Framework;
using System;

namespace Chirpel.Test
{
    public class Tests
    {
        private UserController _userController;
        [SetUp]
        public void Setup()
        {
            _userController = new UserController();
        }

        [Test]
        public void TestCase1CreatePost()
        {
            _userController = new UserController();
            LoginUser user = new LoginUser() { Username = "test", Password = "test" };
            ApiResponse loginResponse = _userController.PostLogin(user);
            if (!loginResponse.Succes)
                Assert.Fail("login failed");

            Random random = new Random();

            PostController postController = new PostController();
            ApiResponse postResponse = postController.Post(new NewPost() { Content = random.Next().ToString(), Token = loginResponse.Message });
            if (!postResponse.Succes)
                Assert.Fail($"Post Failed response: {postResponse.Message}");

            UIPost post = postController.Get(postResponse.Message);
            if (post == null)
                Assert.Fail("post not created");

            if (post.PostId != postResponse.Message)
                Assert.Fail("post.id != created post id");

            Assert.Pass();
        }


        [Test]
        public void TestRegister()
        {
            _userController = new UserController();
            
            Random random = new Random();
            string randomstr = random.Next().ToString();

            RegisterUser registerUser = new RegisterUser() { Username = randomstr, Email = randomstr+"@gmail.com", Password = "password"};

            ApiResponse RegResponse = _userController.PostRegister(registerUser);

            while (!RegResponse.Succes)
            { 
                if (RegResponse.Message != "username" && RegResponse.Message != "password")
                Assert.Fail($"Register Failed error: {RegResponse.Message}");
                randomstr = random.Next().ToString(); 

                registerUser = new RegisterUser() { Username = randomstr, Email = randomstr + "@gmail.com", Password = "password" };
                RegResponse = _userController.PostRegister(registerUser);
            }

            LoginUser loginUser = new LoginUser() { Username = randomstr, Password = "password" };
            ApiResponse LogRepsonse = _userController.PostLogin(loginUser);

            Assert.IsTrue(LogRepsonse.Succes);
        }
        
        [Test]
        public void TestLogin()
        {
            LoginUser user = new LoginUser() { Username = "test", Password = "test" };
            ApiResponse LogResponse = _userController.PostLogin(user);
            if (!LogResponse.Succes)
                Assert.Fail("login failed");

            ApiResponse VerResponse = _userController.VerifyUser(new VerificationToken() {Value = LogResponse.Message });

            Assert.IsTrue(VerResponse.Succes, $"Message {VerResponse.Message}");
        }


        [Test]
        public void TestLoginFail()
        {
            LoginUser user = new LoginUser() { Username = "test", Password = "tes123" };
            ApiResponse LogResponse = _userController.PostLogin(user);
            if (LogResponse.Succes)
                Assert.Fail($"login succesfull {LogResponse.Message}");

            Assert.True(LogResponse.Message == "password");
        }




    }
}