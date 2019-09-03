using AudioTAGEditor.Models;

namespace AudioTAGEditor.Services
{
    public interface IID3Service
    {
        AudioFile GetTag(string filePath);
        TagVersion GetTagVersion(string filePath);
        bool HasTag(string filePath);
    }
}