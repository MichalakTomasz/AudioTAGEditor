﻿using AudioTAGEditor.Models;
using AutoMapper;
using IdSharp.Tagging.ID3v1;
using System;
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

        public Audiofile GetTag(string filePath)
        {
            var hasTag = ID3v1Tag.DoesTagExist(filePath);
            var filename = Path.GetFileName(filePath);
            var genres = genreService.GetID3v1Genres();

            if (hasTag)
            { 
                var tag = new ID3v1Tag(filePath);
                var audioFile = mapper.Map(tag, new Audiofile(Guid.NewGuid()));
                audioFile.HasTag = true;
                audioFile.Filename = filename;
                audioFile.TagType = TagType.ID3V1;
                audioFile.TagVersion = GetTagVersion(filePath);
                audioFile.Genre = genres[tag.GenreIndex];

                return audioFile;
            }
            else
            {
                return new Audiofile(Guid.NewGuid())
                {
                    HasTag = false,
                    Filename = filename
                };
            }
        }

        public IReadOnlyList<string> GetGenres()
            => genreService.GetID3v1Genres();

        public void UpdateTag(
            Audiofile audioFile, 
            string filePath)
        {

            var genres = genreService.GetID3v1Genres();
            var genreIndex = genres.ToList().FindIndex(g => g == audioFile.Genre);

            var id3v1Tag = mapper.Map(audioFile, new ID3v1Tag(filePath));
            id3v1Tag.GenreIndex = genreIndex;
            try
            {
                id3v1Tag.Save(filePath);
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
                ID3v1Tag.RemoveTag(filePath);
            }
            catch (Exception e)
            {
                throw new Exception("Removing ID3v1 error");
            }
        }
    }
}
