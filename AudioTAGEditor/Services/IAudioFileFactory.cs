using AudioTAGEditor.Models;
using AudioTAGEditor.ViewModels;

namespace AudioTAGEditor.Services
{
    public interface IAudioFileFactory
    {
        AudioFile CreateAudioFile(AudioFileViewModel audioFileViewModel);
    }
}