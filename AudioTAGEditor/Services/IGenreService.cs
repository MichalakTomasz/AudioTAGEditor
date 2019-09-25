using System.Collections.Generic;

namespace AudioTAGEditor.Services
{
    public interface IGenreService
    {
        IReadOnlyList<string> GetID3v1Genres();
    }
}