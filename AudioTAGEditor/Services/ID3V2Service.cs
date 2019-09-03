using AudioTAGEditor.Models;
using IdSharp.Tagging.ID3v2;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioTAGEditor.Services
{
    class ID3V2Service
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
                    TrackNumber = GetDigitsFromTrackNumber(tag.TrackNumber),
                    Genre = tag.Genre,
                    Comment = tag.CommentsList[0].Value,
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
            return trackNumber
                .Trim()
                .Where(t => char.IsDigit(t))
                .ToString();
        }
    }  
}
