using System;
using System.IO;

namespace AudioTAGEditor.Services
{
    public class FileService : IFileService
    {
        public void ChangeFilename(string path, string newFilename)
        {
            try
            {
                var filePath = Path.GetDirectoryName(path);
                var filename = Path.GetFileNameWithoutExtension(path);
                var extension = Path.GetExtension(path);
                var newFilePath = $"{filePath}{newFilename}{extension}";
                File.Move(path, newFilePath);
            }
            catch (Exception e)
            {

            }
        }
    }
}
