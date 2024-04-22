using Entities.DTOs;
using Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagerApi.Data;
using TaskManagerApi.Services;

namespace TaskManagerApi.Controllers
{
    [Authorize(Roles = "Admin")]
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly UserService _userService;
        private readonly AccountService _accountService;

        public UsersController(AppDbContext appDbContext, UserService userService, AccountService accountService)
        {
            _db = appDbContext;
            _userService = userService;
            _accountService = accountService;
        }

        [HttpGet("all")]
        public IActionResult GetUsers()
        {
            IEnumerable<UserDTO> users = _db.Users.Select(u => u.ToUserDTO());

            if (users == null) return NotFound();

            return Ok(users);
        }

        [HttpGet("{id}")]
        public IActionResult GetUser(int id)
        {
            User u = _db.Users.FirstOrDefault(x => x.Id == id);

            if (u == null) return NotFound();

            return Ok(u.ToUserDTO());
        }

        [HttpPost]
        public IActionResult CreateUser([FromBody] UserDTO userModel)
        {
            if (userModel == null) return BadRequest();

            bool result = _userService.Create(userModel);

            return result ? Ok() : NotFound();
        }

        [HttpPatch("{id}")]
        public IActionResult UpdateUser(int id, [FromBody] UserDTO userModel)
        {
            if (userModel == null) return BadRequest();

            bool result = _userService.Update(id, userModel);

            return result ? Ok() : NotFound();
        }

        [HttpDelete(("{id}"))]
        public IActionResult DeleteUser(int id)
        {
            bool result = _userService.Delete(id);

            return result ? Ok() : NotFound();
        }

        
        [HttpPost("mCreate")]
        public async Task<IActionResult> MultipleUserCreate([FromBody] IEnumerable<UserDTO> userDTOs)
        {
            if (userDTOs == null || userDTOs.Count() == 0) return BadRequest();

            bool result = await _userService.MultipleUserCreate(userDTOs);

            return result ? Ok() : NotFound();
        }
    }
}
