using AudioTAGEditor.Models;

namespace AudioTAGEditor.Services
{
    public interface IAudiofileComparerService
    {
        bool AreTheSame(Audiofile sourcefile, Audiofile fileToCompare);
    }
}