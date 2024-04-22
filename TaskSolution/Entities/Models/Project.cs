using Entities.Abstractions;
using Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class Project : CommonObject
    {
        public int? AdminId { get; set; }
        public ProjectAdmin Admin { get; set; }
        public List<User> Users { get; set; } = new List<User>();
        public List<Desk> Desks { get; set; } = new List<Desk>();
        public ProjectStatus Status { get; set; }
    }
}
