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
    public class TaskRequestServiceTests
    {
        [TestMethod()]
        public void GetTasksByDeskIdTest()
        {
            UserRequestService service = new UserRequestService();
            var token = service.GetToken("admin", "qwerty123");

            var res = new TaskRequestService().GetTasksByDeskId(token.token, 1);
            Console.WriteLine(res.tasks.Count);
            Assert.AreEqual(HttpStatusCode.OK, res.code);
        }

        [TestMethod()]
        public void GetUsersTasksTest()
        {
            UserRequestService service = new UserRequestService();
            var token = service.GetToken("admin", "qwerty123");

            var res = new TaskRequestService().GetUsersTasks(token.token);
            Console.WriteLine(res.tasks.Count);
            Assert.AreEqual(HttpStatusCode.OK, res.code);
        }

        [TestMethod()]
        public void GetTaskByIdTest()
        {
            UserRequestService service = new UserRequestService();
            var token = service.GetToken("admin", "qwerty123");

            var res = new TaskRequestService().GetTaskById(token.token, 2);
            Console.WriteLine(res.task.Name);
            Assert.AreEqual(HttpStatusCode.OK, res.code);
        }

        [TestMethod()]
        public async Task CreateTaskTest()
        {
            UserRequestService service = new UserRequestService();
            var token = service.GetToken("admin", "qwerty123");

            TaskDTO task = new()
            {
                Name = "TestTask",
                Description = "TestTask Description",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(7),
                DeskId = 1,
                Column = "New",
                ExecutorId = 1
            };

            var res = await new TaskRequestService().CreateTask(token.token, task);

            Assert.AreEqual(HttpStatusCode.OK, res);
        }

        [TestMethod()]
        public async Task UpdateTaskTest()
        {
            UserRequestService service = new UserRequestService();
            var token = service.GetToken("admin", "qwerty123");

            TaskDTO task = new()
            {
                Id = 3,
                Name = "TestTask Updated",
                Description = "TestTask Description Updated",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(7),
                DeskId = 1,
                Column = "New",
                ExecutorId = 1
            };

            var res = await new TaskRequestService().UpdateTask(token.token, task);

            Assert.AreEqual(HttpStatusCode.OK, res);
        }

        [TestMethod()]
        public async Task DeleteDeskTest()
        {
            UserRequestService service = new UserRequestService();
            var token = service.GetToken("admin", "qwerty123");

            var res = await new TaskRequestService().DeleteTask(token.token, 3);

            Assert.AreEqual(HttpStatusCode.OK, res);
        }
    }
}
