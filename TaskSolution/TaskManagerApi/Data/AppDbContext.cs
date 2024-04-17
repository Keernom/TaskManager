using Entities.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagerApi.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectAdmin> ProjectAdmins { get; set; }
        public DbSet<TaskModel> Tasks {  get; set; }
        public DbSet<Desk> Desks { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> contextOptions) : base(contextOptions)
        {
            Database.EnsureDeleted();
            Database.EnsureCreated();

            if (!Users.Any(u => u.Status == UserStatus.Admin))
            {
                var admin = new User()
                {
                    FirstName = "AdminUser",
                    LastName = "Adminich",
                    Email = "admin",
                    Password = "qwerty123",
                    Status = UserStatus.Admin,
                    RegistrationDate = DateTime.Now,
                };

                Users.Add(admin);
                SaveChanges();
            }
        }
    }
}
