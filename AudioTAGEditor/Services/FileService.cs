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
                var newFilePath = $"{filePath}{Path.DirectorySeparatorChar}{newFilename}";
                File.Move(path, newFilePath);
            }
            catch (Exception e)
            {

            }
        }
    }
}
