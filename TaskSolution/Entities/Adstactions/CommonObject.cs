using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Adstactions
{
    abstract class CommonObject
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public byte[] Image { get; set; }

        public CommonObject()
        {
            CreatedDate = DateTime.Now;
        }
    }
}
