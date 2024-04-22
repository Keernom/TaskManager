using Entities.Abstractions;
using Entities.DTOs;
using Entities.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TaskManagerApi.Data;

namespace TaskManagerApi.Services
{
    public class UserService : ICommonService<UserDTO>
    {
        private readonly AppDbContext _db;

        public UserService(AppDbContext dbContext)
        {
            _db = dbContext;
        }

        public User GetUser(string login, string password)
        {
            return _db.Users.FirstOrDefault(u => u.Email == login && u.Password == password);
        }

        public ClaimsIdentity GetIdentity(string username, string password)
        {
            User currentUser = GetUser(username, password);
            if (currentUser != null)
            {
                currentUser.LastLogin = DateTime.UtcNow;
                _db.Users.Update(currentUser);
                _db.SaveChanges();

                var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, currentUser.Email),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, currentUser.Status.ToString())
                };
                ClaimsIdentity claimsIdentity =
                new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                    ClaimsIdentity.DefaultRoleClaimType);
                return claimsIdentity;
            }

            // если пользователь не найдено
            return null;
        }

        public bool Create(UserDTO model)
        {
            return DoAction(delegate ()
            {
                User newUser = new User(model.FirstName, model.LastName, model.Email,
                model.Password, model.PhoneNumber, model.Image, model.Status);

                _db.Users.Add(newUser);
                _db.SaveChanges();
            });
        }

        public bool Update(int id, UserDTO model)
        {
            return DoAction(delegate ()
            {
                User u = _db.Users.FirstOrDefault(x => x.Id == id);

                u.FirstName = model.FirstName;
                u.LastName = model.LastName;
                u.Email = model.Email;
                u.Password = model.Password;
                u.PhoneNumber = model.PhoneNumber;
                u.Image = model.Image;
                u.Status = model.Status;

                _db.Users.Update(u);
                _db.SaveChanges();
            });
        }

        public bool Delete(int id)
        {
            return DoAction(delegate () {
                User u = _db.Users.FirstOrDefault(x => x.Id == id);

                _db.Users.Remove(u);
                _db.SaveChanges();
            });
        }

        public async Task<bool> MultipleUserCreate(IEnumerable<UserDTO> userDTOs)
        {
            if (userDTOs == null || userDTOs.Count() == 0) return false;

            IEnumerable<User> users = userDTOs.Select(u => new User(u));
            await _db.AddRangeAsync(users);

            await _db.SaveChangesAsync();

            return true;
        }

        public bool DoAction(Action action)
        {
            if (action == null) return false;

            try
            {
                action();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
    }
}
