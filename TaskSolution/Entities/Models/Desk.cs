using Entities.Adstactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    internal class Desk : CommonObject
    {
        public bool IsPrivate { get; set; }
        public string Columns { get; set; }
        public User Admin { get; set; }
    }
}
