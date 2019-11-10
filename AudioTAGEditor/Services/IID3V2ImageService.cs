using System;
using System.Collections.Generic;
using System.Windows.Media.Imaging;

namespace AudioTAGEditor.Services
{
    public interface IID3V2ImageService
    {
        bool HasImages(string filepath);
        IEnumerable<BitmapImage> GetImages(string filepath);
    }
}
