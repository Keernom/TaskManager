using Entities.Abstractions;
using Entities.DTOs;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using TaskManagerApi.Data;
using TaskManagerApi.Services.Abstracts;

namespace TaskManagerApi.Services
{
    public class DeskService : AbstractService, ICommonService<DeskDTO>
    {
        private readonly AppDbContext _db;

        public DeskService(AppDbContext dbContext)
        {
            _db = dbContext;
        }

        public bool Create(DeskDTO model)
        {
            return DoAction(delegate ()
            {
                Desk desk = new Desk(model);

                _db.Desks.Add(desk);
                _db.SaveChanges();
            });
        }

        public bool Delete(int id)
        {
            return DoAction(delegate ()
            {
                Desk desk = _db.Desks.FirstOrDefault(x => x.Id == id);

                _db.Desks.Remove(desk);
                _db.SaveChanges();
            });
        }

        public DeskDTO Get(int id)
        {
            Desk desk  = _db.Desks.Include(d => d.Tasks).FirstOrDefault(x => x.Id == id);
            var deskDTO = desk?.ToDto();

            if (deskDTO != null)
            {
                deskDTO.TasksIds = desk?.Tasks.Select(t => t.Id).ToList();
            }

            return deskDTO;
        }

        public bool Update(int id, DeskDTO model)
        {
            return DoAction(delegate () 
            {
                Desk desk = _db.Desks.FirstOrDefault(x => x.Id == id);

                desk.Name = model.Name;
                desk.Description = model.Description;
                desk.Image = model.Image;
                desk.AdminId = model.AdminId;
                desk.IsPrivate = model.IsPrivate;
                desk.ProjectId = model.ProjectId;
                desk.Columns = JsonConvert.SerializeObject(model.Columns);

                _db.Desks.Update(desk);
                _db.SaveChanges();
            });
        }

        public async Task<IEnumerable<CommonDTO>> GetAll(int userId)
        {
            return await _db.Desks
                .Where(d => d.AdminId == userId)
                .Select(d => d.ToCommonDTO())
                .ToListAsync();
        }

        public async Task<IEnumerable<CommonDTO>> GetProjectDesks(int projectId, int userId)
        {
            return await _db.Desks
                .Where(d => d.ProjectId == projectId && (d.AdminId == userId || !d.IsPrivate))
                .Select(d => d.ToCommonDTO())
                .ToListAsync();
        }
    }
}
