using Entities.Abstractions;
using Entities.Enums;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTOs
{
    public class ProjectDTO : CommonDTO
    {
        public int? AdminId { get; set; }
        public ProjectStatus Status { get; set; }
        public List<UserDTO> Users { get; set; } = new List<UserDTO>();
        public List<DeskDTO> Desks { get; set; } = new List<DeskDTO>();
    }
}
