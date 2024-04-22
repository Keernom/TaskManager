using Entities.DTOs;
using Entities.Enums;
using Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManagerApi.Data;
using TaskManagerApi.Migrations;
using TaskManagerApi.Services;

namespace TaskManagerApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly UserService _userService;
        private readonly ProjectService _projectService;

        public ProjectController(AppDbContext appDbContext, UserService userService, ProjectService projectService)
        {
            _db = appDbContext;
            _userService = userService;
            _projectService = projectService;
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            ProjectDTO project = _projectService.Get(id);
            return project == null ? NotFound() : Ok(project);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var user = _userService.GetUser(HttpContext.User.Identity.Name);

            if (user.Status == UserStatus.Admin)
            {
                var projects = await _projectService.GetAll();
                return Ok(projects);
            }
            else
            {
                var projects = await _projectService.GetByUserId(user.Id);
                return Ok(projects);
            }

        }

        [HttpPost]
        public IActionResult Create([FromBody] ProjectDTO projectDTO)
        {
            if (projectDTO == null) return BadRequest();

            var user = _userService.GetUser(HttpContext.User.Identity.Name);

            if (user == null) return Unauthorized();

            if (user.Status == UserStatus.Admin || user.Status == UserStatus.Editor)
                return Unauthorized();

            var admin = _db.ProjectAdmins.FirstOrDefault(admin => admin.Id == user.Id);
            if (admin == null) _db.ProjectAdmins.Add(new ProjectAdmin(user));
            projectDTO.AdminId = admin.Id;

            bool result = _projectService.Create(projectDTO);

            return result ? Ok() : NotFound();
        }

        [HttpPatch("{id}")]
        public IActionResult Update(int id, [FromBody] ProjectDTO projectDTO)
        {
            if (projectDTO == null) return BadRequest();

            var user = _userService.GetUser(HttpContext.User.Identity.Name);

            if (user == null) return Unauthorized();

            if (user.Status == UserStatus.Admin || user.Status == UserStatus.Editor)
                return Unauthorized();

            bool result = _projectService.Update(id, projectDTO);

            return result ? Ok() : NotFound();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var user = _userService.GetUser(HttpContext.User.Identity.Name);

            if (user == null) return Unauthorized();

            if (user.Status == UserStatus.Admin || user.Status == UserStatus.Editor)
                return Unauthorized();

            bool result = _projectService.Delete(id);

            return result ? Ok() : NotFound();
        }

        [HttpPatch("{id}/users")]
        public IActionResult AddUsersToProj(int id, [FromBody] List<int> usersIds)
        {
            if (usersIds == null) return BadRequest();

            var user = _userService.GetUser(HttpContext.User.Identity.Name);

            if (user == null) return Unauthorized();

            if (user.Status == UserStatus.Admin || user.Status == UserStatus.Editor)
                return Unauthorized();

            _projectService.AddUsersToProject(id, usersIds);

            return Ok();
        }

        [HttpPatch("{id}/users/remove/{userId}")]
        public IActionResult RemoveUsersFromProj(int id, [FromBody] List<int> usersIds)
        {
            if (usersIds == null) return BadRequest();

            var user = _userService.GetUser(HttpContext.User.Identity.Name);

            if (user == null) return Unauthorized();

            if (user.Status == UserStatus.Admin || user.Status == UserStatus.Editor)
                return Unauthorized();

            _projectService.RemoveUsersFromProj(id, usersIds);

            return Ok();
        }
    }
}
