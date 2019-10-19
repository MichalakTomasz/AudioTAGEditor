using AudioTAGEditor.Models;
using AudioTAGEditor.ViewModels;
using AutoMapper;
using Prism.Events;
using System.Collections.Generic;
using System.Linq;

namespace AudioTAGEditor.Services
{
    public class AudiofileConverter : IAudiofileConverter
    {
        private readonly IMapper mapper;
        public AudiofileConverter(IMapper mapper)
            => this.mapper = mapper;

        public AudiofileViewModel AudiofileToAudiofileViewModel(
            Audiofile audiofile, IEventAggregator eventAggregator)
            => mapper.Map(audiofile, new AudiofileViewModel(audiofile.ID, eventAggregator));

        public IEnumerable<AudiofileViewModel> AudiofilesToAudiofilesViewModel(
            IEnumerable<Audiofile> audiofiles, IEventAggregator eventAggregator)
        {
            var result = new List<AudiofileViewModel>();
            audiofiles.ToList().ForEach(a =>
            {
                var tempAudiofileViewModel = AudiofileToAudiofileViewModel(a, eventAggregator);
                result.Add(tempAudiofileViewModel);
            });

            return result;
        }

        public AudiofileID3v1ViewModel AudiofileToAudiofileID3v1ViewModel(
            Audiofile audiofile, IEventAggregator eventAggregator)
            => mapper.Map(audiofile, new AudiofileID3v1ViewModel(audiofile.ID, eventAggregator));

        public IEnumerable<AudiofileID3v1ViewModel> AudiofilesToAudiofilesID3v1ViewModel(
            IEnumerable<Audiofile> audioFiles, IEventAggregator eventAggregator)
        {
            var result = new List<AudiofileID3v1ViewModel>();
            audioFiles.ToList().ForEach(a =>
            {
                var tempAudiofileID3v1ViewModel = 
                AudiofileToAudiofileID3v1ViewModel(a, eventAggregator);
                result.Add(tempAudiofileID3v1ViewModel);
            });

            return result;
        }

        public Audiofile AudiofileViewModelToAudiofile(
            AudiofileViewModel audiofileViewModel)
            => mapper.Map<Audiofile>(audiofileViewModel);

        public IEnumerable<Audiofile> AudiofilesViewModelToAudiofiles(
            IEnumerable<AudiofileViewModel> audioFilesViewModel)
        {
            var result = new List<Audiofile>();
            audioFilesViewModel.ToList().ForEach(a =>
            {
                var tempAudioFile = AudiofileViewModelToAudiofile(a);
                result.Add(tempAudioFile);
            });

            return result;
        }

        public Audiofile AudiofileID3v1ViewModelToAudiofile(
           AudiofileID3v1ViewModel audioFileID3v1ViewModel)
           => mapper.Map<Audiofile>(audioFileID3v1ViewModel);

        public IEnumerable<Audiofile> AudiofilesID3v1ViewModelToAudiofiles(
            IEnumerable<AudiofileViewModel> audioFilesID3v1ViewModel)
        {
            var result = new List<Audiofile>();
            audioFilesID3v1ViewModel.ToList().ForEach(a =>
            {
                var tempAudioFile = AudiofileID3v1ViewModelToAudiofile(a as AudiofileID3v1ViewModel);
                result.Add(tempAudioFile);
            });

            return result;
        }
    }
}
