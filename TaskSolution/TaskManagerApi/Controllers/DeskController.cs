using Entities.DTOs;
using Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagerApi.Data;
using TaskManagerApi.Services;

namespace TaskManagerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DeskController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly UserService _userService;
        private readonly DeskService _deskService;

        public DeskController(AppDbContext dbContext, UserService userService, DeskService deskService)
        {
            _db = dbContext;
            _userService = userService;
            _deskService = deskService;

        }

        [HttpGet]
        public async Task<IActionResult> GetUsersDesk()
        {
            var user = _userService.GetUser(HttpContext.User.Identity.Name);
            if (user == null) return Unauthorized();

            var desks = await _deskService.GetAll(user.Id);

            return Ok(desks);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var desk = _deskService.Get(id);

            return desk == null ? NotFound() : Ok(desk);
        }

        [HttpGet("project")]
        public async Task<IActionResult> GetProjectDesk(int projectId)
        {
            var user = _userService.GetUser(HttpContext.User.Identity.Name);
            if (user == null) return Unauthorized();

            var desks = await _deskService.GetProjectDesks(projectId, user.Id);

            return desks == null ? NotFound() : Ok(desks);
        }

        [HttpPost]
        public IActionResult Create([FromBody] DeskDTO deskDTO)
        {
            if (deskDTO == null) return BadRequest();

            var user = _userService.GetUser(HttpContext.User.Identity.Name);
            if (user == null) return Unauthorized();

            var result = _deskService.Create(deskDTO);

            return result ? Ok() : NotFound();
        }

        [HttpPatch("{id}")]
        public IActionResult Update(int id, [FromBody] DeskDTO deskDTO)
        {
            if (deskDTO == null) return BadRequest();

            var user = _userService.GetUser(HttpContext.User.Identity.Name);
            if (user == null) return Unauthorized();

            var result = _deskService.Update(id, deskDTO);

            return result ? Ok() : NotFound();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var result = _deskService.Delete(id);

            return result ? Ok() : NotFound();
        }
    }
}
