using Entities.Abstractions;
using Entities.DTOs;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using TaskManagerApi.Data;
using TaskManagerApi.Services.Abstracts;

namespace TaskManagerApi.Services
{
    public class ProjectService : AbstractService, ICommonService<ProjectDTO>
    {
        private readonly AppDbContext _db;
        private readonly UserService _userService;

        public ProjectService(AppDbContext db, UserService userService)
        {
            _db = db;
            _userService = userService;
        }

        public bool Create(ProjectDTO model)
        {
            return DoAction(delegate () 
            {
                Project project = new Project(model);
                _db.Projects.Add(project);
                _db.SaveChanges();
            });
        }

        public bool Delete(int id)
        {
            return DoAction(delegate ()
            {
                Project project = _db.Projects.FirstOrDefault(p => p.Id == id);
                _db.Projects.Remove(project);
                _db.SaveChanges();
            });
        }

        public ProjectDTO Get(int id)
        {
            Project project = _db.Projects.Include(p => p.Users).FirstOrDefault(p => p.Id == id);
            var projectDTO = project?.ToDto();

            if (projectDTO != null)
            {
                projectDTO.UsersIds = project.Users.Select(u => u.Id).ToList();
            }

            return projectDTO;
        }

        public async Task<IEnumerable<CommonDTO>> GetAll()
        {
            return await _db.Projects.Select(p => p.ToDto() as CommonDTO).ToListAsync();
        }

        public async Task<IEnumerable<ProjectDTO>> GetByUserId(int userId)
        {
            List<ProjectDTO> result = new List<ProjectDTO>();

            var admin = _db.ProjectAdmins.FirstOrDefault(a => a.UserId == userId);

            if (admin != null)
            {
                var adminProjects = await _db.Projects.Where(p => p.AdminId == admin.Id).Select(p => p.ToDto()).ToListAsync();
                result.AddRange(adminProjects);
            }
            var usersProjects = await _db.Projects
                .Include(p => p.Users)
                .Where(p => p.Users.Any(u => u.Id == userId))
                .Select(p => p.ToDto()).ToListAsync();

            result.AddRange(usersProjects);

            return result;
        }

        public bool Update(int id, ProjectDTO model)
        {
            return DoAction(delegate ()
            {
                Project project = _db.Projects.FirstOrDefault(p => p.Id == id);

                project.Name = model.Name;
                project.Description = model.Description;
                project.Image = model.Image;
                project.Status = model.Status;
                project.AdminId = model.AdminId;

                _db.Projects.Update(project);
                _db.SaveChanges();
            });
        }

        public void AddUsersToProject(int projectId, IEnumerable<int> usersId)
        {
            var project = _db.Projects.Include(p => p.Users).FirstOrDefault(p => p.Id == projectId);

            if (project == null) return;

            foreach (var userId in usersId)
            {
                User user = _db.Users.FirstOrDefault(u => u.Id == userId);
                project.Users.Add(user);
            }

            _db.SaveChanges();
        }

        public void RemoveUsersFromProj(int projectId, IEnumerable<int> usersId)
        {
            var project = _db.Projects.Include(p => p.Users).FirstOrDefault(p => p.Id == projectId);

            foreach (var userId in usersId)
            {
                User user = _db.Users.FirstOrDefault(u => u.Id == userId);

                if (project.Users.Contains(user))
                {
                    project.Users.Remove(user);
                }
            }

            _db.SaveChanges();
        }
    }
}
