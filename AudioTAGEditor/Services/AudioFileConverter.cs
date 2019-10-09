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

        public AudiofileViewModel AudioFileToAudioFileViewModel(
            Audiofile audioFile, IEventAggregator eventAggregator)
            => mapper.Map(audioFile, new AudiofileViewModel(audioFile.ID, eventAggregator));

        public IEnumerable<AudiofileViewModel> AudioFilesToAudioFilesViewModel(
            IEnumerable<Audiofile> audioFiles, IEventAggregator eventAggregator)
        {
            var result = new List<AudiofileViewModel>();
            audioFiles.ToList().ForEach(a =>
            {
                var tempAudioFileViewModel = AudioFileToAudioFileViewModel(a, eventAggregator);
                result.Add(tempAudioFileViewModel);
            });

            return result;
        }

        public AudiofileID3v1ViewModel AudioFileToAudioFileID3v1ViewModel(
            Audiofile audioFile, IEventAggregator eventAggregator)
            => mapper.Map(audioFile, new AudiofileID3v1ViewModel(audioFile.ID, eventAggregator));

        public IEnumerable<AudiofileID3v1ViewModel> AudioFilesToAudioFilesID3v1ViewModel(
            IEnumerable<Audiofile> audioFiles, IEventAggregator eventAggregator)
        {
            var result = new List<AudiofileID3v1ViewModel>();
            audioFiles.ToList().ForEach(a =>
            {
                var tempAudioFileID3v1ViewModel = 
                AudioFileToAudioFileID3v1ViewModel(a, eventAggregator);
                result.Add(tempAudioFileID3v1ViewModel);
            });

            return result;
        }

        public Audiofile AudioFileViewModelToAudioFile(
            AudiofileViewModel audioFileViewModel)
            => mapper.Map<Audiofile>(audioFileViewModel);

        public IEnumerable<Audiofile> AudioFilesViewModelToAudioFiles(
            IEnumerable<AudiofileViewModel> audioFilesViewModel)
        {
            var result = new List<Audiofile>();
            audioFilesViewModel.ToList().ForEach(a =>
            {
                var tempAudioFile = AudioFileViewModelToAudioFile(a);
                result.Add(tempAudioFile);
            });

            return result;
        }

        public Audiofile AudioFileID3v1ViewModelToAudioFile(
           AudiofileID3v1ViewModel audioFileID3v1ViewModel)
           => mapper.Map<Audiofile>(audioFileID3v1ViewModel);

        public IEnumerable<Audiofile> AudioFilesID3v1ViewModelToAudioFiles(
            IEnumerable<AudiofileID3v1ViewModel> audioFilesID3v1ViewModel)
        {
            var result = new List<Audiofile>();
            audioFilesID3v1ViewModel.ToList().ForEach(a =>
            {
                var tempAudioFile = AudioFileID3v1ViewModelToAudioFile(a);
                result.Add(tempAudioFile);
            });

            return result;
        }
    }
}
