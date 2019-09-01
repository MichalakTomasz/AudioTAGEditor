using AudioTAGEditor.Models;
using IdSharp.Tagging.ID3v1;
using IdSharp.Tagging.ID3v2;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioTAGEditor.Services
{
    class AudioTAGService : IAudioTAGService
    {
        public AudioFile GetTagData(string filePath, TAGType tagType)
        {
            switch (tagType)
            {
                case TAGType.ID3V1:
                    return GetID3V1Tag(filePath);
                case TAGType.ID3V2:
                    return GetID3V2Tag(filePath);
                default: return default(AudioFile);
            }
        }

        AudioFile GetID3V1Tag(string filePath)
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
                    Genre = GenreHelper.GenreByIndex[tag.GenreIndex],
                    Comment = tag.Comment
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

        AudioFile GetID3V2Tag(string filePath)
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
                    Genre = tag.Genre,
                    Comment = tag.CommentsList[0].Value
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
