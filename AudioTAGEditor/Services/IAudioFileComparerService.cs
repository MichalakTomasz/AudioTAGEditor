using AudioTAGEditor.Models;

namespace AudioTAGEditor.Services
{
    public interface IAudioFileComparerService
    {
        bool AreTheSame(AudioFile sourceFile, AudioFile fileToCompare);
    }
}