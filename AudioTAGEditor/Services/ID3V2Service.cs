using AudioTAGEditor.Models;
using AutoMapper;
using IdSharp.Tagging.ID3v2;
using System;
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
            var genres = genreService.GetID3v1Genres();

            if (hasTag)
            {
                var tag = new ID3v2Tag(filePath);
                var audioFile = mapper.Map<AudioFile>(tag);
                audioFile.HasTag = true;
                audioFile.Filename = filename;
                audioFile.TagType = TagType.ID3V2;

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

        private string GetDigitsFromTrackNumber(string trackNumber)
        {
            var result = trackNumber
                .Trim()
                .Where(t => char.IsDigit(t)).ToArray();
            return new string(result);
        }

        public TagVersion GetTagVersion(string filePath)
            => TagVersion.unknown;

        public IReadOnlyList<string> GetGenres()
            => genreService.GetID3v1Genres();

        public void UpdateTag(
            AudioFile audioFile, 
            string filePath, 
            TagVersion tagVersion)
        {
            var id3v2Tag = mapper.Map(audioFile, new ID3v2Tag(filePath));
            try
            {
                id3v2Tag.Save(filePath);
            }
            catch (Exception e)
            {
                throw new Exception("Saving ID3v2 error");
            }
        }

        public void RemoveTag(string filePath)
        {
            try
            {
                ID3v2Tag.RemoveTag(filePath);
            }
            catch (Exception e)
            {
                throw new Exception("Removing ID3v2 error");
            }
        }
    }  
}
