using Chirpel.Controllers;
using Chirpel.Data.FakeDAL;
using Chirpel.Logic;
using Chirpel.Logic.Account;
using Chirpel.Logic.Message;
using Chirpel.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Chirpel.Test
{
    public class IntegrationTests
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

            RegisterUser registerUser = new RegisterUser() { Username = randomstr, Email = randomstr + "@gmail.com", Password = "password" };

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

            ApiResponse VerResponse = _userController.VerifyUser(new VerificationToken() { Value = LogResponse.Message });

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

        [Test]
        public void TestRemoveUser()
        {
            UserLogic user = new UserLogic();
            user.GetByPassword("password");
            if (user.Id == null)
                Assert.Fail("No user found");

            ApiResponse LogResponse = _userController.PostLogin(new LoginUser() { Username = user.Username, Password = user.Password });

            if (!LogResponse.Succes)
                Assert.Fail($"login failed error: {LogResponse.Message}");

            ApiResponse DelResponse = _userController.PostDelete(new VerificationToken() { Value = LogResponse.Message });

            if (!DelResponse.Succes)
                Assert.Fail($"Delete failed error: {DelResponse.Message}");

            UserLogic newUser = new UserLogic();
            newUser.GetById(user.Id);
            Assert.IsNull(newUser.Id);
        }

        [Test]
        public void TestRemovePost()
        {
            UserLogic user = new UserLogic();
            user.GetByUsername("test");

            PostCollection postCollection = new PostCollection();
            postCollection.GetAllPostsFromUser(user.Id);

            string postId = postCollection.Posts[0].Id;
            PostLogic postLogic = new PostLogic(postCollection.Posts[0].Id, postCollection.Posts[0].Content, postCollection.Posts[0].User, postCollection.Posts[0].PostDate);
            postCollection.DeletePost(postLogic);
            
            PostLogic test = new PostLogic();
            test.GetById(postId);

            Assert.IsNotNull(test.Id);
        }
    }

    public class UnitTests
    {
        private FakePostLikesDAL fakePostLikesDAL;
        private FakePostDAL fakePostDAL;
        private FakeUserDAL fakeUserDAL;
        private FakeUserFollowersDAL fakeUserFollowerDAL;
        private FakeUserSettingsDAL fakeUserSettingDAL;

        [SetUp]
        public void Setup()
        {
            fakePostLikesDAL = new FakePostLikesDAL();
            fakePostDAL = new FakePostDAL();
            fakeUserDAL = new FakeUserDAL();
            fakeUserFollowerDAL = new FakeUserFollowersDAL();
            fakeUserSettingDAL = new FakeUserSettingsDAL();
        }
        
        [Test]
        public void TestPostLikesAdd()
        {
            Random random = new Random();

            string Username = random.Next().ToString();
            string Password = random.Next().ToString();
            string Email = random.Next().ToString();
            
            string Content = random.Next().ToString();

            UserCollection userCollection = new UserCollection(fakeUserDAL, fakeUserFollowerDAL, fakeUserSettingDAL);
            Response res = userCollection.Register(Username, Email, Password);
            
            if (!res.Succes)
                Assert.Fail($"Register failed msg: {res.Message}");

            UserLogic userLogic = new UserLogic(fakeUserDAL, fakeUserFollowerDAL, fakeUserSettingDAL);
            userLogic.GetByUsername(Username);

            Assert.IsNotNull(userLogic.Id, "user login failed");

            PostCollection postCollection = new PostCollection(fakePostDAL, fakePostLikesDAL);
            Response response = postCollection.AddPost(Content, userLogic);

            if (!response.Succes)
                Assert.Fail($"AddPost failed msg: {response.Message}");


            string Username1 = random.Next().ToString();
            string Password1 = random.Next().ToString();
            string Email1 = random.Next().ToString();

           
            UserCollection userCollection1 = new UserCollection(fakeUserDAL, fakeUserFollowerDAL, fakeUserSettingDAL);
            Response res1 = userCollection1.Register(Username1, Email1, Password1);

            if (!res1.Succes)
                Assert.Fail($"Register failed msg: {res1.Message}");

            UserLogic userLogic1 = new UserLogic(fakeUserDAL, fakeUserFollowerDAL, fakeUserSettingDAL);
            userLogic1.GetByUsername(Username);

            PostLogic postLogic = new PostLogic(fakePostDAL, fakePostLikesDAL);
            postLogic.GetById(postCollection.Posts[0].Id);
            Response response1 = postLogic.Like(userLogic1);

            if (!response1.Succes)
                Assert.Fail($"like failed msg: {response1.Message}");

            Assert.IsNotNull(fakePostLikesDAL.PostLikes.Find(c => c.PostId == postLogic.Id && c.UserId == userLogic1.Id));
        }

        [Test]
        public void TestPostAdd()
        {
            Random random = new Random();

            string Username = random.Next().ToString();
            string Password = random.Next().ToString();
            string Email = random.Next().ToString();

            string Content = random.Next().ToString();

            UserCollection userCollection = new UserCollection(fakeUserDAL, fakeUserFollowerDAL, fakeUserSettingDAL);
            Response res = userCollection.Register(Username, Email, Password);

            if (!res.Succes)
                Assert.Fail($"Register failed msg: {res.Message}");

            UserLogic userLogic = new UserLogic(fakeUserDAL, fakeUserFollowerDAL, fakeUserSettingDAL);
            userLogic.GetByUsername(Username);

            Assert.IsNotNull(userLogic.Id, "user login failed");

            PostCollection postCollection = new PostCollection(fakePostDAL, fakePostLikesDAL);
            Response response = postCollection.AddPost(Content, userLogic);

            if (!response.Succes)
                Assert.Fail($"AddPost failed msg: {response.Message}");

            Assert.IsNotNull(fakePostDAL.Posts.Find(c => c.Content == Content && c.UserId == userLogic.Id));
        }

        [Test]
        public void TestUserSettingsAdd()
        {
            Random random = new Random();

            string Username = random.Next().ToString();
            string Password = random.Next().ToString();
            string Email = random.Next().ToString();

            UserCollection userCollection = new UserCollection(fakeUserDAL, fakeUserFollowerDAL, fakeUserSettingDAL);
            Response res = userCollection.Register(Username, Email, Password);

            
            if (!res.Succes)
                Assert.Fail($"Register failed msg: {res.Message}");

            UserLogic userLogic = new UserLogic(fakeUserDAL, fakeUserFollowerDAL, fakeUserSettingDAL);
            userLogic.GetByUsername(Username);


            Assert.IsNotNull(fakeUserSettingDAL.UserSettings.Find(c => c.Id == userLogic.Id));
        }

        [Test]
        public void TestUserAdd()
        {
            Random random = new Random();

            string Username = random.Next().ToString();
            string Password = random.Next().ToString();
            string Email = random.Next().ToString();

            UserCollection userCollection = new UserCollection(fakeUserDAL, fakeUserFollowerDAL, fakeUserSettingDAL);
            Response res = userCollection.Register(Username, Email, Password);

            if (!res.Succes)
                Assert.Fail($"Register failed msg: {res.Message}");

            Assert.IsNotNull(fakeUserDAL.Users.Find(c => c.Username == Username && c.Email == Email && c.Password == Password));
        }

        [Test]
        public void TestUserFollowerAdd()
        {
            Random random = new Random();

            string Username = random.Next().ToString();
            string Password = random.Next().ToString();
            string Email = random.Next().ToString();

            UserCollection userCollection = new UserCollection(fakeUserDAL, fakeUserFollowerDAL, fakeUserSettingDAL);
            Response res = userCollection.Register(Username, Email, Password);

            if (!res.Succes)
                Assert.Fail($"Register failed msg: {res.Message}");

            UserLogic userLogic = new UserLogic(fakeUserDAL, fakeUserFollowerDAL, fakeUserSettingDAL);
            userLogic.GetByUsername(Username);

            string Username1 = random.Next().ToString();
            string Password1 = random.Next().ToString();
            string Email1 = random.Next().ToString();

            UserCollection userCollection1 = new UserCollection(fakeUserDAL, fakeUserFollowerDAL, fakeUserSettingDAL);
            Response res1 = userCollection.Register(Username1, Email1, Password1);

            if (!res1.Succes)
                Assert.Fail($"Register1 failed msg: {res1.Message}");

            UserLogic userLogic1 = new UserLogic(fakeUserDAL, fakeUserFollowerDAL, fakeUserSettingDAL);
            userLogic1.GetByUsername(Username);

            userLogic.FollowUser(userLogic1);

            Assert.IsNotNull(fakeUserFollowerDAL.userFollowers.Find(c => c.Follower == userLogic.Id && c.Followed == userLogic1.Id));
        }
    }
}