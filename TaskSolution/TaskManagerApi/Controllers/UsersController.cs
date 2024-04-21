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

        [HttpPost("create")]
        public IActionResult CreateUser([FromBody] UserDTO userModel)
        {
            if (userModel == null) return BadRequest();

            User newUser = new User(userModel.FirstName, userModel.LastName, userModel.Email,
                userModel.Password, userModel.PhoneNumber, userModel.Image, userModel.Status);

            _db.Users.Add(newUser);
            _db.SaveChanges();

            return Ok();
        }

        [HttpPatch("{id}/update")]
        public IActionResult UpdateUser(int id, [FromBody] UserDTO userModel)
        {
            if (userModel == null) return BadRequest();

            User u = _db.Users.FirstOrDefault(x => x.Id == id);

            if (u == null) return NotFound();

            u.FirstName = userModel.FirstName;
            u.LastName = userModel.LastName;
            u.Email = userModel.Email;
            u.Password = userModel.Password;
            u.PhoneNumber = userModel.PhoneNumber;
            u.Image = userModel.Image;
            u.Status = userModel.Status;

            _db.Users.Update(u);
            _db.SaveChanges();

            return Ok();
        }

        [HttpDelete(("{id}"))]
        public IActionResult DeleteUser(int id)
        {
            User u = _db.Users.FirstOrDefault(x => x.Id == id);

            if (u == null) return NotFound();

            _db.Users.Remove(u);
            _db.SaveChanges();

            return Ok();
        }

        
        [HttpPost("mCreate")]
        public async Task<IActionResult> MultipleUserCreate([FromBody] IEnumerable<UserDTO> userDTOs)
        {
            if (userDTOs == null || userDTOs.Count() == 0) return BadRequest();

            IEnumerable<User> users = userDTOs.Select(u => new User(u));
            await _db.AddRangeAsync(users);

            await _db.SaveChangesAsync();

            return Ok();
        }
    }
}
