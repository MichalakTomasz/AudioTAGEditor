using AudioTAGEditor.Models;
using IdSharp.Tagging.ID3v1;
using System.Collections.Generic;
using System.IO;

namespace AudioTAGEditor.Services
{
    class ID3V1Service : IID3Service
    {
        private readonly IGenreService genreService;

        public ID3V1Service(IGenreService genreService)
            => this.genreService = genreService;

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
            var filename = Path.GetFileName(filePath);
            if (hasTag)
            {
                var tag = new ID3v1Tag(filePath);
                return new AudioFile
                {
                    HasTag = true,
                    Filename = filename,
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
                    Filename = filename
                };
            }
        }

        public IDictionary<int, string> GetGenres()
            => genreService.GetID3v1Genres();
            
    }
}
