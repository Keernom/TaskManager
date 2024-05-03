using Entities.DTOs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using TaskManagerWPF.Models.Extensions;

namespace TaskManagerWPF.Models
{
    public class TaskClient
    {
        public TaskDTO Model { get; private set; }
        public TaskClient(TaskDTO model)
        {
            Model = model;
        }

        public UserDTO Creator { get; set; }
        public UserDTO Executor { get; set; }
        public BitmapImage Image
        {
            get
            {
                return Model.LoadImage();
            }
        }
        public bool IsHaveFile
        {
            get => Model?.File != null;
        }

    }
}
