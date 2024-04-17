using Entities.Adstactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    internal class Project : CommonObject
    {
        public List<User> Users { get; set; }
        public List<Desk> Desks { get; set; }
    }
}
