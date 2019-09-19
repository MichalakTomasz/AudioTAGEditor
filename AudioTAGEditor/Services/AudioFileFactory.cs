using AudioTAGEditor.Models;
using AudioTAGEditor.ViewModels;
using AutoMapper;

namespace AudioTAGEditor.Services
{
    public class AudioFileFactory : IAudioFileFactory
    {
        private readonly IMapper mapper;
        public AudioFileFactory(IMapper mapper)
            => this.mapper = mapper;

        public AudioFile CreateAudioFile(AudioFileViewModel audioFileViewModel)
            => mapper.Map<AudioFile>(audioFileViewModel);
    }
}
