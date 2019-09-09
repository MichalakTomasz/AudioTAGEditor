using AudioTAGEditor.Models;
using System.Collections.Generic;

namespace AudioTAGEditor.Services
{
    public interface IID3Service
    {
        AudioFile GetTag(string filePath);
        TagVersion GetTagVersion(string filePath);
        bool HasTag(string filePath);
        IDictionary<int, string> GetGenres();
    }
}