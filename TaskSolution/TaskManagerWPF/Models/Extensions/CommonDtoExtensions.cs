﻿using Entities.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace TaskManagerWPF.Models.Extensions
{
    public static class CommonDtoExtensions
    {
        public static BitmapImage LoadImage(this CommonDTO model)
        {
            if (model?.Image == null || model?.Image.Length == 0) return null;

            var image = new BitmapImage();
            using (var memSm = new MemoryStream(model.Image))
            {
                memSm.Position = 0;
                image.BeginInit();
                image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.UriSource = null;
                image.StreamSource = memSm;
                image.EndInit();
            }

            image.Freeze();
            return image;
        }
    }
}
