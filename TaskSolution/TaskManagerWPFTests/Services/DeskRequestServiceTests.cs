using Entities.DTOs;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TaskManagerWPF.Services;

namespace TaskManagerWPFTests.Services.Tests
{
    [TestClass()]
    public class DeskRequestServiceTests
    {
        [TestMethod()]
        public void GetUsersDesksTest()
        {
            UserRequestService service = new UserRequestService();
            var token = service.GetToken("admin", "qwerty123");

            var res = new DeskRequestService().GetUsersDesks(token.token);
            Console.WriteLine(res.desks.Count);
            Assert.AreEqual(HttpStatusCode.OK, res.code);
        }

        [TestMethod()]
        public void GetDeskByIdTest()
        {
            UserRequestService service = new UserRequestService();
            var token = service.GetToken("admin", "qwerty123");

            var res = new DeskRequestService().GetDeskById(token.token, 1);
            Console.WriteLine(res.desk.Name);
            Assert.AreEqual(HttpStatusCode.OK, res.code);
        }

        [TestMethod()]
        public void GetDesksByProjectTest()
        {
            UserRequestService service = new UserRequestService();
            var token = service.GetToken("admin", "qwerty123");

            var res = new DeskRequestService().GetDesksByProject(token.token, 1);
            Console.WriteLine(res.desks.Count);
            Assert.AreEqual(HttpStatusCode.OK, res.code);
        }

        [TestMethod()]
        public async Task CreateDeskTest()
        {
            UserRequestService service = new UserRequestService();
            var token = service.GetToken("admin", "qwerty123");

            DeskDTO desk = new()
            {
                Name = "TestDesk",
                Description = "TestDesk Description",
                IsPrivate = true,
                Columns = new[] { "New", "InProcess", "Finished" },
                ProjectId = 1,
                AdminId = 1
            };

            var res = await new DeskRequestService().CreateDesk(token.token, desk);

            Assert.AreEqual(HttpStatusCode.OK, res);
        }

        [TestMethod()]
        public async Task UpdateDeskTest()
        {
            UserRequestService service = new UserRequestService();
            var token = service.GetToken("admin", "qwerty123");

            DeskDTO desk = new()
            {
                Id = 2,
                Name = "TestDesk Updated",
                Description = "TestDesk Description Updated",
                IsPrivate = true,
                Columns = new[] { "New", "InProcess", "Finished" },
                ProjectId = 1,
                AdminId = 1
            };

            var res = await new DeskRequestService().UpdateDesk(token.token, desk);

            Assert.AreEqual(HttpStatusCode.OK, res);
        }

        [TestMethod()]
        public async Task DeleteDeskTest()
        {
            UserRequestService service = new UserRequestService();
            var token = service.GetToken("admin", "qwerty123");

            var res = await new DeskRequestService().DeleteDesk(token.token, 2);

            Assert.AreEqual(HttpStatusCode.OK, res);
        }
    }
}
