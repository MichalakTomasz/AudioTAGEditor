using AudioTAGEditor.Models;
using IdSharp.Tagging.ID3v1;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioTAGEditor.Services
{
    class ID3V1Service
    {
        public bool HasTag(string filePath)
            => ID3v1Tag.DoesTagExist(filePath);

        public TagVersion GetTagVersion(string filePath)
        {
            if (HasTag(filePath))
            {
                var tag = new ID3v1Tag(filePath);

                switch (tag.TagVersion)
                {
                    case ID3v1TagVersion.ID3v10:
                        return TagVersion.ID3V10;
                    case ID3v1TagVersion.ID3v11:
                        return TagVersion.ID3V11;
                }
            }
            return TagVersion.none;
        }

        public AudioFile GetTag(string filePath)
        {
            var hasTag = ID3v1Tag.DoesTagExist(filePath);
            var fileName = Path.GetFileName(filePath);
            if (hasTag)
            {
                var tag = new ID3v1Tag(filePath);
                return new AudioFile
                {
                    HasTag = true,
                    FileName = fileName,
                    Title = tag.Title,
                    Artist = tag.Artist,
                    Album = tag.Album,
                    TrackNumber = tag.TrackNumber.ToString(),
                    Genre = GenreHelper.GenreByIndex[tag.GenreIndex],
                    Comment = tag.Comment,
                    Year = tag.Year,
                    TagType = TagType.ID3V1,
                    TagVersion = GetTagVersion(filePath)
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
    }
}
