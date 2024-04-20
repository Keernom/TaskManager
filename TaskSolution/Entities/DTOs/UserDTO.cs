using Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTOs
{
    public class UserDTO
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
        public UserStatus Status { get; set; }

        public UserDTO()
        {
            
        }

        public UserDTO(string fname, string lname, string email, string password,
            string phone, UserStatus status)
        {
            FirstName = fname;
            LastName = lname;
            Email = email;
            Password = password;
            PhoneNumber = phone;
            Status = status;
            RegistrationDate = DateTime.Now;
        }
    }
}
