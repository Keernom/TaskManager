using Entities.Abstractions;
using Entities.DTOs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class Desk : CommonObject
    {
        public bool IsPrivate { get; set; }
        public string Columns { get; set; }
        public int AdminId { get; set; }
        public User Admin { get; set; }
        public int ProjectId { get; set; }
        public Project Project { get; set; }
        public List<TaskModel> Tasks { get; set; } = new List<TaskModel>();

        public Desk() { }

        public Desk(DeskDTO deskDTO) : base(deskDTO) 
        {
            IsPrivate = deskDTO.IsPrivate;
            AdminId = deskDTO.AdminId;
            ProjectId = deskDTO.ProjectId;

            if (deskDTO.Columns.Any())
                Columns = JsonConvert.SerializeObject(deskDTO.Columns);
        }

        public DeskDTO ToDto()
        {
            return new DeskDTO()
            {
                Id = Id,
                Name = Name,
                Description = Description,
                CreatedDate = CreatedDate,
                Image = Image,
                AdminId = AdminId,
                IsPrivate = IsPrivate,
                Columns = JsonConvert.DeserializeObject<string[]>(Columns),
                ProjectId = ProjectId,
            };
        }

        public CommonDTO ToCommonDTO()
        {
            return new CommonDTO()
            {
                Id = Id,
                Name = Name,
                Description = Description,
                CreatedDate = CreatedDate,
                Image = Image
            };
        }
    }
}
