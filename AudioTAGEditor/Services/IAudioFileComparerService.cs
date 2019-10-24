using AudioTAGEditor.Models;
using System.Collections.Generic;

namespace AudioTAGEditor.Services
{
    public interface IAudiofileComparerService
    {
        bool AreTheSame(Audiofile sourcefile, Audiofile fileToCompare);
        bool AreTheSame(IEnumerable<Audiofile> sourceAudiofiles, IEnumerable<Audiofile> filesToCompare);
        bool AreTheSameTags(Audiofile sourcefile, Audiofile fileToCompare);
        bool AreTheSameTags(IEnumerable<Audiofile> sourceAudiofiles, IEnumerable<Audiofile> filesToCompare);
    }
}