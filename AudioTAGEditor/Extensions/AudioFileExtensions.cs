using AudioTAGEditor.Models;
using AudioTAGEditor.ViewModels;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioTAGEditor.Extensions
{
    public static class AudioFileExtensions
    {
        public static AudioFileViewModel ToAudioFileViewModel(this AudioFile audioFile, IEventAggregator eventAggregator)
        {
            var audioFileViewModel = new AudioFileViewModel(eventAggregator)
            {

            };
            return audioFileViewModel;
        }
    }
}
