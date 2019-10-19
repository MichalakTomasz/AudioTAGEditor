namespace AudioTAGEditor.Services
{
    public interface IFileService
    {
        void Rename(string path, string newFilename);
        bool Exist(string fullFilepath);
    }
}