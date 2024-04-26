using Entities.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TaskManagerApi.Data;
using TaskManagerApi.Services;

namespace TaskManagerApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class TaskController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly UserService _userService;
        private readonly TaskService _taskService;

        public TaskController(AppDbContext db, UserService userService, TaskService taskService)
        {
            _db = db;
            _userService = userService;
            _taskService = taskService;
        }

        [HttpGet]
        public async Task<IActionResult> GetTasksByDeskId(int deskId)
        {
            var user = _userService.GetUser(HttpContext.User.Identity.Name);
            if (user == null) return Unauthorized();

            var tasks = await _taskService.GetAll(deskId);

            return Ok(tasks);
        }

        [HttpGet("user")]
        public async Task<IActionResult> GetTasksForCurrentUser()
        {
            var user = _userService.GetUser(HttpContext.User.Identity.Name);
            if (user == null) return Unauthorized();

            var tasks = await _taskService.GetByUser(user.Id);
            return tasks == null ? NotFound() : Ok(tasks);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var user = _userService.GetUser(HttpContext.User.Identity.Name);
            if (user == null) return Unauthorized();

            var task = _taskService.Get(id);

            return task == null ? NotFound() : Ok(task);
        }

        [HttpPost]
        public IActionResult Create([FromBody] TaskDTO taskDTO)
        {
            if (taskDTO == null) return BadRequest();

            var user = _userService.GetUser(HttpContext.User.Identity.Name);
            if (user == null) return Unauthorized();

            taskDTO.CreatorId = user.Id;
            var result = _taskService.Create(taskDTO);

            return result ? Ok(result) : NotFound();
        }

        [HttpPatch("{id}")]
        public IActionResult Update(int id, [FromBody] TaskDTO taskDTO)
        {
            if (taskDTO == null) return BadRequest();

            var user = _userService.GetUser(HttpContext.User.Identity.Name);
            if (user == null) return Unauthorized();

            var result = _taskService.Update(id, taskDTO);

            return result ? Ok(result) : NotFound();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteById(int id)
        {
            var result = _taskService.Delete(id);

            return result ? Ok(result) : NotFound();
        }
    }
}
