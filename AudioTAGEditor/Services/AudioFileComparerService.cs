using AudioTAGEditor.Models;
using System.Collections.Generic;
using System.Linq;

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

        public bool AreTheSame(
            IEnumerable<Audiofile> sourceAudiofiles, 
            IEnumerable<Audiofile> filesToCompare)
        {
            if (sourceAudiofiles?.Count() != filesToCompare?.Count()) return false;

            var mergedForLoop = sourceAudiofiles
                .Zip(filesToCompare, (source, toCompare) => new { source, toCompare });

            return !mergedForLoop.Any(i => !AreTheSame(i.source, i.toCompare));
        }

        public bool AreTheSameTags(Audiofile sourcefile, Audiofile fileToCompare)
        {
            return !(sourcefile.Album != fileToCompare.Album ||
                sourcefile.Artist != fileToCompare.Artist ||
                sourcefile.Comment != fileToCompare.Comment ||
                sourcefile.Genre != fileToCompare.Genre ||
                sourcefile.HasTag != fileToCompare.HasTag ||
                sourcefile.TagType != fileToCompare.TagType ||
                sourcefile.TagVersion != fileToCompare.TagVersion ||
                sourcefile.Title != fileToCompare.Title ||
                sourcefile.TrackNumber != fileToCompare.TrackNumber ||
                sourcefile.Year != fileToCompare.Year);
        }

        public bool AreTheSameTags(
            IEnumerable<Audiofile> sourceAudiofiles, 
            IEnumerable<Audiofile> filesToCompare)
        {
            if (sourceAudiofiles?.Count() != filesToCompare?.Count()) return false;

            var mergedForLoop = sourceAudiofiles
                .Zip(filesToCompare, (source, toCompare) => new { source, toCompare });

            return !mergedForLoop.Any(i => !AreTheSameTags(i.source, i.toCompare));
        }
    }
}
