using NUnit.Framework;
using Chirpel;
using Chirpel.Controllers;
using Chirpel.Models;

namespace Chirpel.UnitTests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            UserController userController = new UserController();
            ApiResponse PostResponse = userController.PostRegister(new Models.RegisterUser() { Username = "test", Email = "test", Password = "test@gmail.com" });
            if (PostResponse.Succes)
            {
                ApiResponse VerifyResponse = userController.VerifyUser(new VerificationToken() { Value = PostResponse.Message });
                if (VerifyResponse.Succes)
                    Assert.IsTrue(VerifyResponse.Message == "test", $"actual response{VerifyResponse.Message}");
                else
                    Assert.Fail("Verification Failed");
            }
            Assert.Fail(PostResponse.Message);    

            Assert.Pass();
        }
    }
}