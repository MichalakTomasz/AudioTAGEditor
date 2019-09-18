using AudioTAGEditor.Models;
using AudioTAGEditor.ViewModels;
using Prism.Events;

namespace AudioTAGEditor.Services
{
    public interface IAudioFileViewModelFactory
    {
        AudioFileViewModel CreateFromAudioFile(AudioFile audioFile, IEventAggregator eventAggregator);
    }
}