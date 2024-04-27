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
    public class ProjectRequestServiceTests
    {
        [TestMethod()]
        public void GetAllProjectsTest()
        {
            UserRequestService service = new UserRequestService();
            var token = service.GetToken("admin", "qwerty123");

            var res = new ProjectRequestService().GetAllProjects(token.token);
            Console.WriteLine(res.projects.Count);
            Assert.AreEqual(HttpStatusCode.OK, res.code);
        }

        [TestMethod()]
        public void GetProjectByIdTest()
        {
            UserRequestService service = new UserRequestService();
            var token = service.GetToken("admin", "qwerty123");

            var res = new ProjectRequestService().GetProjectById(token.token, 1);
            Console.WriteLine(res.project.Name);
            Assert.AreEqual(HttpStatusCode.OK, res.code);
        }

        [TestMethod()]
        public async Task CreateProjectTest()
        {
            UserRequestService service = new UserRequestService();
            var token = service.GetToken("admin", "qwerty123");

            ProjectDTO project = new()
            {
                Name = "TestProject",
                Description = "TestProject Description",
                Status = 0
            };

            var res = await new ProjectRequestService().CreateProject(token.token, project);

            Assert.AreEqual(HttpStatusCode.OK, res);
        }

        [TestMethod()]
        public async Task UpdateProjectTest()
        {
            UserRequestService service = new UserRequestService();
            var token = service.GetToken("admin", "qwerty123");

            ProjectDTO project = new()
            {
                Id = 2, // HARD CODED
                Name = "TestProject Updated",
                Description = "TestProject Description Updated",
                Status = 0
            };

            var res = await new ProjectRequestService().UpdateProject(token.token, project);

            Assert.AreEqual(HttpStatusCode.OK, res);
        }

        [TestMethod()]
        public async Task AddUsersToProjectTest()
        {
            UserRequestService service = new UserRequestService();
            var token = service.GetToken("admin", "qwerty123");

            var res = await new ProjectRequestService().AddUsersToProject(token.token, 2, new List<int> { 4, 5});

            Assert.AreEqual(HttpStatusCode.OK, res);
        }

        [TestMethod()]
        public async Task RemoveUsersFromProjectTest()
        {
            UserRequestService service = new UserRequestService();
            var token = service.GetToken("admin", "qwerty123");

            var res = await new ProjectRequestService().RemoveUsersFromProject(token.token, 2, new List<int> { 4 });

            Assert.AreEqual(HttpStatusCode.OK, res);
        }

        [TestMethod()]
        public async Task DeleteProjectTest()
        {
            UserRequestService service = new UserRequestService();
            var token = service.GetToken("admin", "qwerty123");

            var res = await new ProjectRequestService().DeleteProject(token.token, 2);

            Assert.AreEqual(HttpStatusCode.OK, res);
        }
    }
}
