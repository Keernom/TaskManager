using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public byte[] Image { get; set; }
        public List<Project> Projects { get; set; } = new List<Project>(); 
        public List<TaskModel> Tasks { get; set; } = new List<TaskModel>();
        public UserStatus Status { get; set; }
    }
}
