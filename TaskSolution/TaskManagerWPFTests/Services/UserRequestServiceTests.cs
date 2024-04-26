using Entities.DTOs;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;

namespace TaskManagerWPF.Services.Tests
{
    [TestClass()]
    public class UserRequestServiceTests
    {
        [TestMethod()]
        public void GetTokenTest()
        {
            var res = new UserRequestService().GetToken("admin", "qwerty123");
            Assert.IsNotNull(res.token);
        }

        [TestMethod()]
        public async Task CreateUserTest()
        {
            UserRequestService service = new UserRequestService();
            var token = service.GetToken("admin", "qwerty123");

            UserDTO user = new()
            {
                FirstName = "TestFN",
                LastName = "TestLN",
                Email = "test@mail.com",
                Password = "123",
            };

            var code = await service.CreateUser(token.token, user);
            Assert.AreEqual(HttpStatusCode.OK, code);
        }

        [TestMethod()]
        public void GetUsersTest()
        {
            UserRequestService service = new UserRequestService();
            var token = service.GetToken("admin", "qwerty123");

            var res = service.GetUsers(token.token);
            Assert.AreEqual(HttpStatusCode.OK, res.code);
        }

        [TestMethod()]
        public async Task DeleteUserTest()
        {
            UserRequestService service = new UserRequestService();
            var token = service.GetToken("admin", "qwerty123");

            var res = await service.DeleteUser(token.token, 3);
            Assert.AreEqual(HttpStatusCode.OK, res);
        }

        [TestMethod()]
        public async Task CreateMultipleUsersTest()
        {
            UserRequestService service = new UserRequestService();
            var token = service.GetToken("admin", "qwerty123");

            var userlist = new List<UserDTO>() 
            {
                new()
                {
                    FirstName = "TestFN",
                    LastName = "TestLN",
                    Email = "test@mail.com",
                    Password = "123",
                },
                new()
                {
                    FirstName = "TestFN2",
                    LastName = "TestLN2",
                    Email = "test2@mail.com",
                    Password = "123",
                },
            };

            var res = await service.CreateMultipleUsers(token.token, userlist);
            Assert.AreEqual(HttpStatusCode.OK, res);
        }

        [TestMethod()]
        public async Task UpdateUserTest()
        {
            UserRequestService service = new UserRequestService();
            var token = service.GetToken("admin", "qwerty123");

            UserDTO user = new()
            {
                Id = 4,  // HARD CODED
                FirstName = "TestFN Updated",
                LastName = "TestLN Updated",
                Email = "test@mail.com",
                Password = "123",
            };

            var code = await service.UpdateUser(token.token, user);
            Assert.AreEqual(HttpStatusCode.OK, code);
        }
    }
}