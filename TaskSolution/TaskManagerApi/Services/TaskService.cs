using Entities.Abstractions;
using Entities.DTOs;
using Entities.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;
using TaskManagerApi.Data;
using TaskManagerApi.Services.Abstracts;
using static System.Net.Mime.MediaTypeNames;

namespace TaskManagerApi.Services
{
    public class TaskService : AbstractService, ICommonService<TaskDTO>
    {
        private readonly AppDbContext _db;

        public TaskService(AppDbContext dbContext)
        {
            _db = dbContext;
        }

        public bool Create(TaskDTO modelDto)
        {
            return DoAction(delegate()
            {
                TaskModel model = new TaskModel(modelDto);

                _db.Tasks.Add(model);
                _db.SaveChanges();
            });
        }

        public bool Delete(int id)
        {
            return DoAction(delegate ()
            {
                TaskModel model = _db.Tasks.FirstOrDefault(t => t.Id == id);

                _db.Tasks.Remove(model);
                _db.SaveChanges();
            });
        }

        public TaskDTO Get(int id)
        {
            TaskModel model = _db.Tasks.FirstOrDefault(t => t.Id == id);
            return model?.ToDto();
        }

        public bool Update(int id, TaskDTO model)
        {
            return DoAction(delegate ()
            {
                TaskModel taskModel = _db.Tasks.FirstOrDefault(t => t.Id == id);

                taskModel.Name = model.Name;
                taskModel.Description = model.Description;
                taskModel.Image = model.Image;
                taskModel.StartDate = model.StartDate;
                taskModel.EndDate = model.EndDate;
                taskModel.File = model.File;
                taskModel.DeskId = model.DeskId;
                taskModel.Column = model.Column;
                taskModel.CreatorId = model.CreatorId;
                taskModel.ExecutorId = model.ExecutorId;

                _db.Tasks.Update(taskModel);
                _db.SaveChanges();
            });
        }

        public async Task<IEnumerable<TaskModel>> GetAll(int deskId)
        {
            return await _db.Tasks
                .Where(t => t.DeskId == deskId)
                .Select(t => t.ToShortDto())
                .ToListAsync();
        }

        public async Task<IEnumerable<TaskModel>> GetByUser(int userId)
        {
            return await _db.Tasks
                .Where(t => t.CreatorId == userId || t.ExecutorId == userId)
                .Select(t => t.ToShortDto())
                .ToListAsync();
        }
    }
}
