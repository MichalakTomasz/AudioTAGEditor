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
        public AudioFile GetTagData(string filePath, TagType tagType)
        {
            switch (tagType)
            {
                case TagType.ID3V1:
                    return GetID3V1Tag(filePath);
                case TagType.ID3V2:
                    return GetID3V2Tag(filePath);
                default: return default(AudioFile);
            }
        }

        public bool HasTAGV1TAG(string filePath)
            => ID3v1Tag.DoesTagExist(filePath);

        public bool HasTAGV2TAG(string filePath)
            => ID3v2Tag.DoesTagExist(filePath);

        public TagType ID3V1TAGVesrsion(string filePath)
        {
            if (HasTAGV1TAG(filePath))
            {
                var tag = new ID3v1Tag(filePath);
                
                switch (tag.TagVersion)
                {
                    case ID3v1TagVersion.ID3v10:
                        return TagType.ID3V10;
                    case ID3v1TagVersion.ID3v11:
                        return TagType.ID3V11;
                }
            }
            return TagType.none;
        }

        //public TAGType ID3V2TAGVersion(string filePath)
        //{
        //    if (HasTAGV2TAG(filePath))
        //    {
        //        var tag = new ID3v2Tag(filePath);
               
        //        switch (tag)
        //        {
        //            default:
        //                break;
        //        }
        //    }
        //    return TAGType.none;
        //}

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
                    TrackNumber = tag.TrackNumber.ToString(),
                    Genre = GenreHelper.GenreByIndex[tag.GenreIndex],
                    Comment = tag.Comment,
                    Year = tag.Year
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
                    TrackNumber = tag.TrackNumber,
                    Genre = tag.Genre,
                    Comment = tag.CommentsList[0].Value,
                    Year = tag.Year
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
