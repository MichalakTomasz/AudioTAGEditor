using AudioTAGEditor.Models;
using AudioTAGEditor.ViewModels;
using AutoMapper;
using Prism.Events;
using System.Collections.Generic;
using System.Linq;

namespace AudioTAGEditor.Services
{
    public class AudioFileConverter : IAudioFileConverter
    {
        private readonly IMapper mapper;
        public AudioFileConverter(IMapper mapper)
            => this.mapper = mapper;

        public AudioFileViewModel AudioFileToAudioFileViewModel(
            AudioFile audioFile, IEventAggregator eventAggregator)
            => mapper.Map(audioFile, new AudioFileViewModel(audioFile.ID, eventAggregator));

        public IEnumerable<AudioFileViewModel> AudioFilesToAudioFilesViewModel(
            IEnumerable<AudioFile> audioFiles, IEventAggregator eventAggregator)
        {
            var result = new List<AudioFileViewModel>();
            audioFiles.ToList().ForEach(a =>
            {
                var tempAudioFileViewModel = AudioFileToAudioFileViewModel(a, eventAggregator);
                result.Add(tempAudioFileViewModel);
            });

            return result;
        }

        public AudioFileID3v1ViewModel AudioFileToAudioFileID3v1ViewModel(
            AudioFile audioFile, IEventAggregator eventAggregator)
            => mapper.Map(audioFile, new AudioFileID3v1ViewModel(audioFile.ID, eventAggregator));

        public IEnumerable<AudioFileID3v1ViewModel> AudioFilesToAudioFilesID3v1ViewModel(
            IEnumerable<AudioFile> audioFiles, IEventAggregator eventAggregator)
        {
            var result = new List<AudioFileID3v1ViewModel>();
            audioFiles.ToList().ForEach(a =>
            {
                var tempAudioFileID3v1ViewModel = AudioFileToAudioFileID3v1ViewModel(a, eventAggregator);
                result.Add(tempAudioFileID3v1ViewModel);
            });

            return result;
        }

        public AudioFile AudioFileViewModelToAudioFile(
            AudioFileViewModel audioFileViewModel)
            => mapper.Map<AudioFile>(audioFileViewModel);

        public IEnumerable<AudioFile> AudioFilesViewModelToAudioFiles(IEnumerable<AudioFileViewModel> audioFilesViewModel)
        {
            var result = new List<AudioFile>();
            audioFilesViewModel.ToList().ForEach(a =>
            {
                var tempAudioFile = AudioFileViewModelToAudioFile(a);
                result.Add(tempAudioFile);
            });

            return result;
        }

        public AudioFile AudioFileID3v1ViewModelToAudioFile(
           AudioFileID3v1ViewModel audioFileID3v1ViewModel)
           => mapper.Map<AudioFile>(audioFileID3v1ViewModel);

        public IEnumerable<AudioFile> AudioFilesID3v1ViewModelToAudioFiles(IEnumerable<AudioFileID3v1ViewModel> audioFilesID3v1ViewModel)
        {
            var result = new List<AudioFile>();
            audioFilesID3v1ViewModel.ToList().ForEach(a =>
            {
                var tempAudioFile = AudioFileID3v1ViewModelToAudioFile(a);
                result.Add(tempAudioFile);
            });

            return result;
        }
    }
}
