using AudioTAGEditor.Models;
using System.Collections.Generic;
using System.Linq;

namespace AudioTAGEditor.Services
{
    public class AudiofileCloneService : IAudiofieleCloneService
    {
        public Audiofile Clone(Audiofile audiofile)
        {
            return new Audiofile(audiofile.ID)
            {
                Album = audiofile.Album,
                Artist = audiofile.Artist,
                Comment = audiofile.Comment,
                Filename = audiofile.Filename,
                Genre = audiofile.Genre,
                HasTag = audiofile.HasTag,
                TagType = audiofile.TagType,
                TagVersion = audiofile.TagVersion,
                Title = audiofile.Title,
                TrackNumber = audiofile.TrackNumber,
                Year = audiofile.Year
            };
        }

        public IEnumerable<Audiofile> Clone(IEnumerable<Audiofile> audiofiles)
        {
            var resultCollection = new List<Audiofile>();
            audiofiles.ToList().ForEach(a =>
            {
                var tempAudiofile = Clone(a);
                resultCollection.Add(tempAudiofile);
            });

            return resultCollection;
        }
    }
}
