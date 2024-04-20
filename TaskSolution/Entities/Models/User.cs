using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.DTOs;
using Entities.Enums;

namespace Entities.Models
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string? PhoneNumber { get; set; }
        public DateTime RegistrationDate { get; set; }
        public DateTime LastLogin { get; set; }
        public byte[]? Image { get; set; }
        public List<Project> Projects { get; set; } = new List<Project>(); 
        public List<TaskModel> Tasks { get; set; } = new List<TaskModel>();
        public UserStatus Status { get; set; }

        public User()
        {
            
        }

        public User(string fname, string lname, string email, string password,
            string phone = null, byte[] image = null, UserStatus status = UserStatus.User)
        {
            FirstName = fname;
            LastName = lname;
            Email = email;
            Password = password;
            PhoneNumber = phone;
            Status = status;
            RegistrationDate = DateTime.UtcNow;
            Image = image;
            Status = status;
        }

        public User(UserDTO userDTO)
        {
            Id = userDTO.Id;
            FirstName = userDTO.FirstName;
            LastName = userDTO.LastName;
            Email = userDTO.Email;
            Password = userDTO.Password;
            PhoneNumber = userDTO.PhoneNumber;
            RegistrationDate = DateTime.UtcNow;
            LastLogin = userDTO.LastLogin;
            Image = userDTO.Image;
            Status = userDTO.Status;
        }

        public UserDTO ToUserDTO()
        {
            return new UserDTO()
            {
                Id = Id,
                FirstName = FirstName,
                LastName = LastName,
                Email = Email,
                Password = Password,
                PhoneNumber = PhoneNumber,
                RegistrationDate = RegistrationDate,
                LastLogin = LastLogin,
                Image = Image,
                Status = Status
            };
        }
    }
}
