using AudioTAGEditor.Models;
using AutoMapper;
using IdSharp.Tagging.ID3v2;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Media.Imaging;

namespace AudioTAGEditor.Services
{
    public class ID3V2Service : IID3Service, IID3V2ImageService
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

        public Audiofile GetTag(string filePath)
        {
            var hasTag = ID3v2Tag.DoesTagExist(filePath);
            var filename = Path.GetFileName(filePath);

            if (hasTag)
            {
                var tag = new ID3v2Tag(filePath);
                var audioFile = mapper.Map(tag, new Audiofile(Guid.NewGuid()));
                audioFile.HasTag = true;
                audioFile.Filename = filename;
                audioFile.TagType = TagType.ID3V2;
                
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

        public TagVersion GetTagVersion(string filepath)
            => TagVersion.unknown;

        public IReadOnlyList<string> GetGenres()
            => genreService.GetID3v1Genres();

        public void UpdateTag(
            Audiofile audioFile, 
            string filePath)
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

        #region IID3V2ImageService

        public bool HasImages(string filepath)
        {
            if (!ID3v2Tag.DoesTagExist(filepath))
                return false;

            var tag = new ID3v2Tag(filepath);
            return tag.PictureList?.Count > 0;
        }

        public IEnumerable<BitmapImage> GetImages(string filepath)
        {
            if (!ID3v2Tag.DoesTagExist(filepath))
                yield return default;

            var tag = new ID3v2Tag(filepath);
            if (tag.PictureList?.Count == 0)
                yield return default;

            var imageBuffers = tag.PictureList.Select(p => p.PictureData);
            foreach (var item in imageBuffers)
                yield return ConvertToBitmapImage(item);
        }

        private BitmapImage ConvertToBitmapImage(byte[] buffer)
        {
            using (var memoryStream = new MemoryStream(buffer))
            {
                var image = new BitmapImage();
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.StreamSource = memoryStream;
                image.EndInit();
                return image;
            }
        }

        #endregion // IID3V2ImageService
    }
}
