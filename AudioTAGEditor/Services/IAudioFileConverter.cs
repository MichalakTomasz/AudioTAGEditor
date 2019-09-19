using AudioTAGEditor.Models;
using AudioTAGEditor.ViewModels;
using Prism.Events;

namespace AudioTAGEditor.Services
{
    public interface IAudioFileConverter
    {
        AudioFileViewModel AudioFileToAudioFileViewModel(AudioFile audioFile, IEventAggregator eventAggregator);
        AudioFile AdioFileViewModelToAudioFile(AudioFileViewModel audioFileViewModel);
    }
}