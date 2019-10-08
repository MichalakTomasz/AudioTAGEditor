using AudioTAGEditor.Models;
using AudioTAGEditor.ViewModels;
using Prism.Events;
using System.Collections.Generic;

namespace AudioTAGEditor.Services
{
    public interface IAudiofileConverter
    {
        AudiofileViewModel AudioFileToAudioFileViewModel(Audiofile audioFile, IEventAggregator eventAggregator);
        Audiofile AudioFileViewModelToAudioFile(AudiofileViewModel audioFileViewModel);
        IEnumerable<AudiofileViewModel> AudioFilesToAudioFilesViewModel(IEnumerable<Audiofile> audioFiles, IEventAggregator eventAggregator);
        IEnumerable<Audiofile> AudioFilesViewModelToAudioFiles(IEnumerable<AudiofileViewModel> audioFilesViewModel);

        AudiofileID3v1ViewModel AudioFileToAudioFileID3v1ViewModel(Audiofile audioFile, IEventAggregator eventAggregator);
        Audiofile AudioFileID3v1ViewModelToAudioFile(AudiofileID3v1ViewModel audioFileID3v1ViewModel);
        IEnumerable<AudiofileID3v1ViewModel> AudioFilesToAudioFilesID3v1ViewModel(IEnumerable<Audiofile> audioFiles, IEventAggregator eventAggregator);
        IEnumerable<Audiofile> AudioFilesID3v1ViewModelToAudioFiles(IEnumerable<AudiofileID3v1ViewModel> audioFilesID3v1ViewModel);

        HistoryAudiofile AudioFileToHistoryAudioFile(Audiofile audioFile);
        IEnumerable<HistoryAudiofile> AudioFilesToHistoryAudioFiles(IEnumerable<Audiofile> audioiles);
    }
}