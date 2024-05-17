using HundeRallyIdentity;
using HundeRallyIdentity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Drawing.Text;

namespace TestProject1
{
    [TestClass]
    public class UnitTest1(UserManager<IdentityUser> _userManager)
    {

        [TestMethod]
        public async Task CreateUser()
        {
            
            // Arrange
            string email = "TestEmail@Test.com";
            string password = "password";
            string userName = "user";


            // Act
            var user = new IdentityUser();
            user.Email = email;
            user.UserName = userName;

            var result = await _userManager.CreateAsync(user, password);

            // Assert
            var createdUser = await _userManager.FindByEmailAsync(email);
            Assert.IsNotNull(createdUser);
            Assert.AreEqual(email, createdUser.Email);
            Assert.AreEqual(userName, createdUser.UserName);

        }

    }
}