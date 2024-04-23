using Entities.Abstractions;
using Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class TaskModel : CommonObject
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public byte[] File { get; set; }
        public int DeskId { get; set; }
        public Desk Desk { get; set; }
        public string Column { get; set; }
        public int? CreatorId { get; set; }
        public User Creator { get; set; }
        public int? ExecutorId { get; set; }

        public TaskModel()
        {
            
        }

        public TaskModel(TaskDTO taskDTO) : base(taskDTO) 
        { 
            StartDate = taskDTO.StartDate;
            EndDate = taskDTO.EndDate;
            File = taskDTO.File;
            DeskId = taskDTO.DeskId;
            Column = taskDTO.Column;
            CreatorId = taskDTO.CreatorId;
            ExecutorId = taskDTO.ExecutorId;
        }

        public TaskDTO ToDto()
        {
            return new TaskDTO
            {
                Id = Id,
                Name = Name,
                Description = Description,
                CreatedDate = CreatedDate,
                Image = Image,
                StartDate = StartDate,
                EndDate = EndDate,
                File = File,
                DeskId = DeskId,
                Column = Column,
                CreatorId = CreatorId,
                ExecutorId = ExecutorId
            };
        }
    }
}
