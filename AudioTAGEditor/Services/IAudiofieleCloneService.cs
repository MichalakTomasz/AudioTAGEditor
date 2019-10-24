using System.Collections.Generic;
using AudioTAGEditor.Models;

namespace AudioTAGEditor.Services
{
    public interface IAudiofieleCloneService
    {
        Audiofile Clone(Audiofile audiofile);
        IEnumerable<Audiofile> Clone(IEnumerable<Audiofile> audiofiles);
    }
}