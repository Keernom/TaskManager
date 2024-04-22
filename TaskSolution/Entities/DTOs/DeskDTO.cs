using Entities.Abstractions;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTOs
{
    public class DeskDTO : CommonDTO
    {
        public bool IsPrivate { get; set; }
        public string[] Columns { get; set; }
        public int AdminId { get; set; }
        public int ProjectId { get; set; }
        public List<TaskDTO> Tasks { get; set; } = new List<TaskDTO>();
    }
}
