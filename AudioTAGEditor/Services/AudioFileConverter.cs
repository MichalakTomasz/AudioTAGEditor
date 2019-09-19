using AudioTAGEditor.Models;
using AudioTAGEditor.ViewModels;
using AutoMapper;
using Prism.Events;

namespace AudioTAGEditor.Services
{
    public class AudioFileConverter : IAudioFileConverter
    {
        private readonly IMapper mapper;
        public AudioFileConverter(IMapper mapper)
            => this.mapper = mapper;

        public AudioFileViewModel AudioFileToAudioFileViewModel(
            AudioFile audioFile, IEventAggregator eventAggregator)
            => mapper.Map(audioFile, new AudioFileViewModel(eventAggregator));

        public AudioFile AdioFileViewModelToAudioFile(
            AudioFileViewModel audioFileViewModel)
            => mapper.Map<AudioFile>(audioFileViewModel);
    }
}
