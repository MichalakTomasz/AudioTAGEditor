using AudioTAGEditor.Models;

namespace AudioTAGEditor.Services
{
    public interface IAudioTAGService
    {
        AudioFile GetTagData(string filePath, TagType tagType);
    }
}