using AudioTAGEditor.Models;
using AutoMapper;
using IdSharp.Tagging.ID3v1;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AudioTAGEditor.Services
{
    public class ID3V1Service : IID3Service
    {
        private readonly IGenreService genreService;
        private readonly IMapper mapper;

        public ID3V1Service(
            IGenreService genreService,
            IMapper mapper)
        {
            this.genreService = genreService;
            this.mapper = mapper;
        }  

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
            var genres = genreService.GetID3v1Genres();

            if (hasTag)
            { 
                var tag = new ID3v1Tag(filePath);

                var audioFile = new AudioFile
                {
                    HasTag = true,
                    Filename = filename,
                    Title = tag.Title,
                    Artist = tag.Artist,
                    Album = tag.Album,
                    TrackNumber = tag.TrackNumber.ToString(),
                    Genre = genres[tag.GenreIndex],
                    Comment = tag.Comment,
                    Year = tag.Year,
                    TagType = TagType.ID3V1,
                    TagVersion = GetTagVersion(filePath)
                };

                return audioFile;
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

        public void SaveTag(
            AudioFile audioFile, 
            string filePath, 
            TagVersion tagVersion = TagVersion.ID3V11)
        {
            ID3v1TagVersion tagVersionToSave = 
                TagVersion.ID3V10 == tagVersion ? 
                ID3v1TagVersion.ID3v10 : ID3v1TagVersion.ID3v11;

            var genres = genreService.GetID3v1Genres();
            var genreToSave = genres.FirstOrDefault(g => g.Value == audioFile.Genre).Key;

            var changedID3v1Tag = mapper.Map<ID3v1Tag>(audioFile);
            var id3v1Tag = new ID3v1Tag(filePath)
            {
                Title = changedID3v1Tag.Title,
                Album = changedID3v1Tag.Album,
                Artist = changedID3v1Tag.Artist,
                TrackNumber = changedID3v1Tag.TrackNumber,
                GenreIndex = genreToSave,
                Comment = changedID3v1Tag.Comment,
                Year = changedID3v1Tag.Year,
                TagVersion = tagVersionToSave
            };
            id3v1Tag.Save(filePath);
        }
            
    }
}
