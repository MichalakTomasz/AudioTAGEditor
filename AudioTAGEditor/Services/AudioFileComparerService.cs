using AudioTAGEditor.Models;

namespace AudioTAGEditor.Services
{
    public class AudioFileComparerService : IAudioFileComparerService
    {
        public bool AreTheSame(AudioFile sourceFile, AudioFile fileToCompare)
        {
            return !(sourceFile.Album != fileToCompare.Album ||
                sourceFile.Artist != fileToCompare.Artist ||
                sourceFile.Comment != fileToCompare.Comment ||
                sourceFile.Filename != fileToCompare.Filename ||
                sourceFile.Genre != fileToCompare.Genre ||
                sourceFile.HasTag != fileToCompare.HasTag ||
                sourceFile.TagType != fileToCompare.TagType ||
                sourceFile.TagVersion != fileToCompare.TagVersion ||
                sourceFile.Title != fileToCompare.Title ||
                sourceFile.TrackNumber != fileToCompare.TrackNumber ||
                sourceFile.Year != fileToCompare.Year);
        }
    }
}
