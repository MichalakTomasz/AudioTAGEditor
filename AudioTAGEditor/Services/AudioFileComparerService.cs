using AudioTAGEditor.Models;

namespace AudioTAGEditor.Services
{
    public class AudiofileComparerService : IAudiofileComparerService
    {
        public bool AreTheSame(Audiofile sourcefile, Audiofile fileToCompare)
        {
            return !(sourcefile.Album != fileToCompare.Album ||
                sourcefile.Artist != fileToCompare.Artist ||
                sourcefile.Comment != fileToCompare.Comment ||
                sourcefile.Filename != fileToCompare.Filename ||
                sourcefile.Genre != fileToCompare.Genre ||
                sourcefile.HasTag != fileToCompare.HasTag ||
                sourcefile.TagType != fileToCompare.TagType ||
                sourcefile.TagVersion != fileToCompare.TagVersion ||
                sourcefile.Title != fileToCompare.Title ||
                sourcefile.TrackNumber != fileToCompare.TrackNumber ||
                sourcefile.Year != fileToCompare.Year);
        }

        public bool AreTheSameTags(Audiofile sourcefile, Audiofile fileToCompare)
        {
            return !(sourcefile.Album != fileToCompare.Album ||
                sourcefile.Artist != fileToCompare.Artist ||
                sourcefile.Comment != fileToCompare.Comment ||
                sourcefile.Filename != fileToCompare.Filename ||
                sourcefile.Genre != fileToCompare.Genre ||
                sourcefile.HasTag != fileToCompare.HasTag ||
                sourcefile.TagType != fileToCompare.TagType ||
                sourcefile.TagVersion != fileToCompare.TagVersion ||
                sourcefile.Title != fileToCompare.Title ||
                sourcefile.Year != fileToCompare.Year);
        }
    }
}
