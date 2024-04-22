using Entities.Abstractions;
using Entities.DTOs;
using Entities.Models;
using Microsoft.EntityFrameworkCore.Diagnostics;
using TaskManagerApi.Data;
using TaskManagerApi.Services.Abstracts;

namespace TaskManagerApi.Services
{
    public class ProjectService : AbstractService, ICommonService<ProjectDTO>
    {
        private readonly AppDbContext _db;

        public ProjectService(AppDbContext db)
        {
            _db = db;
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
    }
}
