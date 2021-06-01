using Chirpel.Controllers;
using Chirpel.Logic.User;
using Chirpel.Models;
using NUnit.Framework;

namespace Chirpel.Test
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestCase1()
        {
            UserController userController = new UserController();
            ApiResponse loginResponse = userController.PostLogin(new LoginUser() { Username = "test", Password="test"});
            if (!loginResponse.Succes)
                Assert.Fail("login failed");

            PostController postController = new PostController();
            ApiResponse postResponse = postController.Post(new NewPost() { Content = "test", Token = loginResponse.Message });
            if (!postResponse.Succes)
                Assert.Fail($"Post Failed response: {postResponse.Message}");

            UIPost post = postController.Get(postResponse.Message);
            if (post == null)
                Assert.Fail("post not created");

            if (post.PostId != postResponse.Message)
                Assert.Fail("post.id != created post id");

            Assert.Pass();
        }
    }
}