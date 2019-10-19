using System;
using System.IO;
using System.Linq;

namespace AudioTAGEditor.Services
{
    public class FileService : IFileService
    {
        public void Rename(string path, string newFilename)
        {
            try
            {
                var filename = Path.GetFileNameWithoutExtension(path);
                if (filename == newFilename) return;
                var filePath = Path.GetDirectoryName(path);
                var newFilePath = $"{filePath}{Path.DirectorySeparatorChar}{newFilename}";
                File.Move(path, newFilePath);
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public bool Exist(string fullFilepath)
            => File.Exists(fullFilepath);
    }
}
