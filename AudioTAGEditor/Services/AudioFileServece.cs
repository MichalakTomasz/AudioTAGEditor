using AudioTAGEditor.Models;
using AutoMapper;
using IdSharp.Tagging.ID3v1;
using IdSharp.Tagging.ID3v2;
using System;
using System.IO;

namespace AudioTAGEditor.Services
{
    public class AudioFileServece
    {
        private readonly IMapper mapper;

        public AudioFileServece(IMapper mapper)
        {
            this.mapper = mapper;
        }
        public void ChangeFilename(string path, string newFilename)
        {
            try
            {
                var filePath = Path.GetDirectoryName(path);
                var filename = Path.GetFileNameWithoutExtension(path);
                var extension = Path.GetExtension(path);
                var newFilePath = $"{filePath}{newFilename}{extension}";
                File.Move(path, newFilePath);
            }
            catch (System.Exception)
            {

                throw;
            }
        }

        public void UpdateTagData(string path, AudioFile fileData)
        {
            switch (fileData.TagType)
            {
                case TagType.ID3V1:
                    var id3v1File = mapper.Map(fileData, new ID3v2Tag(path));
                    id3v1File.Save(path);
                    break;
                case TagType.ID3V2:
                    var id3v2File = mapper.Map(fileData, new ID3v2Tag(path));
                    id3v2File.Save(path);
                    break;
            }
        }

        public void RemoveTag(string path, TagType tagType)
        {
            try
            {
                switch (tagType)
                {
                    case TagType.ID3V1:
                        ID3v1Tag.RemoveTag(path);
                        break;
                    case TagType.ID3V2:
                        ID3v2Tag.RemoveTag(path);
                        break;
                }
            }
            catch(Exception e)
            {
                
            }
        }
    }
}
