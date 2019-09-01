using AudioTAGEditor.Models;

namespace AudioTAGEditor.Services
{
    public interface IAudioTAGService
    {
        AudioFile GetTagData(string filePath, TAGType tagType);
    }
}