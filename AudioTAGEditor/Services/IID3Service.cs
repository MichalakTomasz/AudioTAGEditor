using AudioTAGEditor.Models;
using AudioTAGEditor.ViewModels;
using System.Collections.Generic;

namespace AudioTAGEditor.Services
{
    public interface IID3Service
    {
        Audiofile GetTag(string filePath);
        TagVersion GetTagVersion(string filePath);
        bool HasTag(string filePath);
        IReadOnlyList<string> GetGenres();
        void UpdateTag(
            Audiofile audioFile,
            string filePath);
        void RemoveTag(string filePath);
    }
}