using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FreeLancer.Controllers;
using FreeLancer.Models;
using NUnit.Framework.Legacy;

namespace FreeLancer.Test
{
    [TestFixture]
    public class SecurityControllerTests
    {
        [Test]
        public void Register_AlreadyExistingUser_RedirectsToRegister()
        {
            // Arrange
            var controller = new SecurityController();
            var existingUser = new User { email = "existing@example.com" };

            // Act
            var result = controller.Save(existingUser) as RedirectToRouteResult;

            // Assert
            ClassicAssert.NotNull(result);
            ClassicAssert.AreEqual("Register", result.RouteValues["action"]);
        }

        [Test]
        public void Register_NewUser_RedirectsToLogin()
        {
            // Arrange
            var controller = new SecurityController();
            var newUser = new User { email = "newuser@example.com", password = "password" };

            // Act
            var result = controller.Save(newUser) as RedirectToRouteResult;

            // Assert
            ClassicAssert.NotNull(result);
            ClassicAssert.Equals("Login", result.RouteValues["action"]);
        }
        [Test]
        public void Login_ValidUser_RedirectsToHomeIndex()
        {
            // Arrange
            var controller = new SecurityController();
            var validUser = new User { email = "validuser@example.com", password = "password" };

            // Act
            var result = controller.Login(validUser) as RedirectToRouteResult;

            // Assert
            ClassicAssert.NotNull(result);
            ClassicAssert.Equals("Index", result.RouteValues["action"]);
        }

        [Test]
        public void Login_InvalidUser_ReturnsViewResult()
        {
            // Arrange
            var controller = new SecurityController();
            var invalidUser = new User { email = "invalid@example.com", password = "wrongpassword" };

            // Act
            var result = controller.Login(invalidUser) as ViewResult;

            // Assert
            ClassicAssert.NotNull(result);
            ClassicAssert.Equals("", result.ViewName);
        }

    }
}