using System.Collections.Generic;

namespace AudioTAGEditor.Services
{
    public interface IGenreService
    {
        IDictionary<int, string> GetID3v1Genres();
    }
}