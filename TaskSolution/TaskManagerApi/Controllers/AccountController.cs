using Entities.DTOs;
using Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection.Metadata.Ecma335;
using TaskManagerApi.Auth;
using TaskManagerApi.Data;
using TaskManagerApi.Services;

namespace TaskManagerApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly UserService _userService;
        private readonly AccountService _accountService;

        public AccountController(AppDbContext appDbContext, UserService userService, AccountService accountService)
        {
            _db = appDbContext;
            _userService = userService;
            _accountService = accountService;
        }

        [Authorize]
        [HttpGet("info")]
        public IActionResult GetUserInfo()
        {
            string username = HttpContext.User.Identity.Name;
            var user = _db.Users.FirstOrDefault(u => u.Email == username);

            return user == null ? NotFound() : Ok(user.ToUserDTO());
        }

        [Authorize]
        [HttpPatch("update")]
        public IActionResult UpdateUser([FromBody] UserDTO userModel)
        {
            if (userModel == null) return BadRequest();

            string username = HttpContext.User.Identity.Name;
            User u = _db.Users.FirstOrDefault(x => x.Email == username);

            if (u == null) return NotFound();

            u.FirstName = userModel.FirstName;
            u.LastName = userModel.LastName;
            u.Password = userModel.Password;
            u.PhoneNumber = userModel.PhoneNumber;
            u.Image = userModel.Image;

            _db.Users.Update(u);
            _db.SaveChanges();

            return Ok();
        }

        [HttpPost("token")]
        public IActionResult GetToken()
        {
            var userData = _accountService.GetUserFromBasicAuth(Request);
            string login = userData.Item1;
            string password = userData.Item2;

            var identity = _userService.GetIdentity(login, password);

            if (identity == null)
            {
                return BadRequest("Неправильный логин или пароль");
            }

            var now = DateTime.UtcNow;
            var jwt = new JwtSecurityToken(
                issuer: AuthOptions.ISSUER,
                audience: AuthOptions.AUDIENCE,
                notBefore: now,
                claims: identity.Claims,
                expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var response = new
            {
                access_token = encodedJwt,
                username = identity.Name
            };

            return Ok(response);
        }
    }
}
