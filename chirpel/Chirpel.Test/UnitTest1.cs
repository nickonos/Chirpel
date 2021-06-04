using Chirpel.Controllers;
using Chirpel.Data.FakeDAL;
using Chirpel.Logic;
using Chirpel.Logic.Post;
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
        public void TestUserFollowerAdd()
        {
            FakeUserFollowersDAL fakeUserFollowersDAL = new FakeUserFollowersDAL();        

            Random random = new Random();
            string Follower = random.Next().ToString();
            string Followed = random.Next().ToString();

            UserFollowerLogic userFollowerLogic = new UserFollowerLogic(fakeUserFollowersDAL, Followed, Follower);
            userFollowerLogic.Add();

            Assert.IsNotNull(fakeUserFollowersDAL.userFollowers.Find(c => c.Followed == Followed && c.Follower == Follower));
        }

        [Test]
        public void TestUserAdd()
        {
            FakeUserDAL fakeUserDal = new FakeUserDAL();

            Random random = new Random();
            string Username = random.Next().ToString();
            string Email = random.Next().ToString();
            string Password = random.Next().ToString();

            UserLogic userLogic = new UserLogic(fakeUserDal, Username, Email, Password);

            userLogic.Add();

            Assert.IsNotNull(fakeUserDal.Users.Find(c => c.Username == Username && c.Email == Email && c.Password == Password));
        }


        [Test]
        public void TestUserSettingsAdd()
        {
            FakeUserSettingsDAL fakeUserSettingsDAL = new FakeUserSettingsDAL();

            Random random = new Random();
            string id = random.Next().ToString();

            UserSettingsLogic userSettingsLogic = new UserSettingsLogic(fakeUserSettingsDAL, id);

            userSettingsLogic.Add();

            Assert.IsNotNull(fakeUserSettingsDAL.UserSettings.Find(c => c.Id == id));
        }


        [Test]
        public void TestPostAdd()
        {
            FakePostDAL fakePostDAL = new FakePostDAL();

            Random random = new Random();
            string content = random.Next().ToString();
            string UserId = random.Next().ToString();

            PostLogic postLogic = new PostLogic(fakePostDAL, content, UserId);

            postLogic.Add();

            Assert.IsNotNull(fakePostDAL.Posts.Find(c => c.UserId == UserId && c.Content == content));
        }

        [Test]
        public void TestPostLikesAdd()
        {
            FakePostLikesDAL fakePostLikesDAL = new FakePostLikesDAL();

            Random random = new Random();
            string UserId = random.Next().ToString();
            string PostId = random.Next().ToString();

            PostLikesLogic postLikesLogic = new PostLikesLogic(fakePostLikesDAL, PostId, UserId);

            postLikesLogic.Add();

            Assert.IsNotNull(fakePostLikesDAL.PostLikes.Find(c => c.PostId == PostId && c.UserId == UserId));
        }
    }
}