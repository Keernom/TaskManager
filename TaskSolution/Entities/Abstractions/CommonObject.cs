using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Abstractions
{
    public abstract class CommonObject
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public byte[]? Image { get; set; }

        public CommonObject()
        {
            CreatedDate = DateTime.Now;
        }

        public CommonObject(CommonDTO dTO)
        {
            Id = dTO.Id;
            Name = dTO.Name;
            Description = dTO.Description;
            CreatedDate = dTO.CreatedDate;
            Image = dTO.Image;
        }
    }
}
