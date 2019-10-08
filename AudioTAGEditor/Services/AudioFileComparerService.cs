using AudioTAGEditor.Models;

namespace AudioTAGEditor.Services
{
    public class AudiofileComparerService : IAudiofileComparerService
    {
        public bool AreTheSame(Audiofile sourceFile, Audiofile fileToCompare)
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
