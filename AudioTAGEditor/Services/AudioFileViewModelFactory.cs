using AudioTAGEditor.Models;
using AudioTAGEditor.ViewModels;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioTAGEditor.Services
{
    public class AudioFileViewModelFactory : IAudioFileViewModelFactory
    {
        public AudioFileViewModel CreateFromAudioFile(AudioFile audioFile, IEventAggregator eventAggregator)
        {
            var audioFileViewModel = new AudioFileViewModel(eventAggregator)
            {
                HasTag = audioFile.HasTag,
                TagType = audioFile.TagType,
                TagVersion = audioFile.TagVersion,
                Filename = audioFile.Filename,
                Title = audioFile.Title,
                Album = audioFile.Album,
                Artist = audioFile.Artist,
                Genre = audioFile.Genre,
                TrackNumber = audioFile.TrackNumber,
                Comment = audioFile.Comment,
                Year = audioFile.Year
            };

            return audioFileViewModel;
        }
    }
}
