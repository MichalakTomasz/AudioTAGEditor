using AudioTAGEditor.Models;
using AudioTAGEditor.ViewModels;
using AutoMapper;
using Prism.Events;

namespace AudioTAGEditor.Services
{
    public class AudioFileViewModelFactory : IAudioFileViewModelFactory
    {
        private readonly IMapper mapper;
        public AudioFileViewModelFactory(IMapper mapper)
            => this.mapper = mapper;

        public AudioFileViewModel CreateFromAudioFile(AudioFile audioFile, IEventAggregator eventAggregator)
            => mapper.Map(audioFile, new AudioFileViewModel(eventAggregator));
    }
}
