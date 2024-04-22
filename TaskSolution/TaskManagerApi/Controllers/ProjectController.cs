using Entities.DTOs;
using Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManagerApi.Data;
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

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var projects = await _db.Projects.Select(p => p.ToDto()).ToListAsync();
            return Ok(projects);
        }

        [HttpPost]
        public IActionResult Create([FromBody] ProjectDTO projectDTO)
        {
            if (projectDTO == null) return BadRequest();

            var user = _userService.GetUser(HttpContext.User.Identity.Name);

            if (user != null)
            {
                var admin = _db.ProjectAdmins.FirstOrDefault(admin => admin.Id == user.Id);
                if (admin == null) _db.ProjectAdmins.Add(new ProjectAdmin(user));
                projectDTO.AdminId = admin.Id;
            }

            bool result = _projectService.Create(projectDTO);

            return result ? Ok() : NotFound();
        }

        [HttpPatch("{id}")]
        public IActionResult Update(int id, [FromBody] ProjectDTO projectDTO)
        {
            if (projectDTO == null) return BadRequest();

            bool result = _projectService.Update(id, projectDTO);

            return result ? Ok() : NotFound();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            bool result = _projectService.Delete(id);

            return result ? Ok() : NotFound();
        }
    }
}
