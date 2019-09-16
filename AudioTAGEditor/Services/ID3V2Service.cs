using AudioTAGEditor.Models;
using AudioTAGEditor.ViewModels;
using AutoMapper;
using IdSharp.Tagging.ID3v1;
using IdSharp.Tagging.ID3v2;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AudioTAGEditor.Services
{
    public class ID3V2Service : IID3Service
    {
        private readonly IGenreService genreService;
        private readonly IMapper mapper;

        public ID3V2Service(
            IGenreService genreService,
            IMapper mapper)
        {
            this.genreService = genreService;
            this.mapper = mapper;
        }
            

        public bool HasTag(string filePath)
            => ID3v2Tag.DoesTagExist(filePath);

        public AudioFile GetTag(string filePath)
        {
            var hasTag = ID3v2Tag.DoesTagExist(filePath);
            var filename = Path.GetFileName(filePath);
            if (hasTag)
            {
                var tag = new ID3v2Tag(filePath);
                return new AudioFile
                {
                    HasTag = true,
                    Filename = filename,
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
                    Filename = filename
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

        public IDictionary<int, string> GetGenres()
            => genreService.GetID3v1Genres();
    }  
}
