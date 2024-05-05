using Entities.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using TaskManagerWPF.Models.Extensions;

namespace TaskManagerWPF.Models
{
    public class CommonDtoClient<T> where T : CommonDTO
    {
        public T Model { get; set; }

        public CommonDtoClient(T model)
        {
            Model = model;
        }

        public BitmapImage Image
        {
            get
            {
                return Model?.LoadImage();
            }
        }
    }
}
