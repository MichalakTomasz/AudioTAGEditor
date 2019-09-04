using AudioTAGEditor.Models;
using IdSharp.Tagging.ID3v2;
using System.IO;
using System.Linq;

namespace AudioTAGEditor.Services
{
    class ID3V2Service : IID3Service
    {
        public bool HasTag(string filePath)
            => ID3v2Tag.DoesTagExist(filePath);

        public AudioFile GetTag(string filePath)
        {
            var hasTag = ID3v2Tag.DoesTagExist(filePath);
            var fileName = Path.GetFileName(filePath);
            if (hasTag)
            {
                var tag = new ID3v2Tag(filePath);
                return new AudioFile
                {
                    HasTag = true,
                    FileName = fileName,
                    Title = tag.Title,
                    Artist = tag.Artist,
                    Album = tag.Album,
                    TrackNumber = string.IsNullOrWhiteSpace(tag.TrackNumber) ? 
                        null : GetDigitsFromTrackNumber(tag.TrackNumber),
                    Genre = tag.Genre,
                    Comment = tag.CommentsList?.Count > 0 ? 
                        tag.CommentsList[0].Value : null,
                    Year = tag.Year,
                    TagType = TagType.ID3V2
                };
            }
            else
            {
                return new AudioFile
                {
                    HasTag = false,
                    FileName = fileName
                };
            }
        }

        string GetDigitsFromTrackNumber(string trackNumber)
        {
            var result = trackNumber
                .Trim()
                .Where(t => char.IsDigit(t)).ToArray();
            return new string(result);
        }

        public TagVersion GetTagVersion(string filePath)
            => TagVersion.unknown;
    }  
}
