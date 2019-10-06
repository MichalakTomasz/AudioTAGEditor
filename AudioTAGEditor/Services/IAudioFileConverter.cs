using AudioTAGEditor.Models;
using AudioTAGEditor.ViewModels;
using Prism.Events;
using System.Collections.Generic;

namespace AudioTAGEditor.Services
{
    public interface IAudioFileConverter
    {
        AudioFileViewModel AudioFileToAudioFileViewModel(AudioFile audioFile, IEventAggregator eventAggregator);
        AudioFile AudioFileViewModelToAudioFile(AudioFileViewModel audioFileViewModel);
        IEnumerable<AudioFileViewModel> AudioFilesToAudioFilesViewModel(IEnumerable<AudioFile> audioFiles, IEventAggregator eventAggregator);
        IEnumerable<AudioFile> AudioFilesViewModelToAudioFiles(IEnumerable<AudioFileViewModel> audioFilesViewModel);

        AudioFileID3v1ViewModel AudioFileToAudioFileID3v1ViewModel(AudioFile audioFile, IEventAggregator eventAggregator);
        AudioFile AudioFileID3v1ViewModelToAudioFile(AudioFileID3v1ViewModel audioFileID3v1ViewModel);
        IEnumerable<AudioFileID3v1ViewModel> AudioFilesToAudioFilesID3v1ViewModel(IEnumerable<AudioFile> audioFiles, IEventAggregator eventAggregator);
        IEnumerable<AudioFile> AudioFilesID3v1ViewModelToAudioFiles(IEnumerable<AudioFileID3v1ViewModel> audioFilesID3v1ViewModel);
    }
}