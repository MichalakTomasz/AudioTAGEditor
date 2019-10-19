using AudioTAGEditor.Models;
using AudioTAGEditor.ViewModels;
using Prism.Events;
using System.Collections.Generic;

namespace AudioTAGEditor.Services
{
    public interface IAudiofileConverter
    {
        AudiofileViewModel AudiofileToAudiofileViewModel(Audiofile audioFile, IEventAggregator eventAggregator);
        Audiofile AudiofileViewModelToAudiofile(AudiofileViewModel audiofileViewModel);
        IEnumerable<AudiofileViewModel> AudiofilesToAudiofilesViewModel(IEnumerable<Audiofile> audiofiles, IEventAggregator eventAggregator);
        IEnumerable<Audiofile> AudiofilesViewModelToAudiofiles(IEnumerable<AudiofileViewModel> audiofilesViewModel);

        AudiofileID3v1ViewModel AudiofileToAudiofileID3v1ViewModel(Audiofile audiofile, IEventAggregator eventAggregator);
        Audiofile AudiofileID3v1ViewModelToAudiofile(AudiofileID3v1ViewModel audiofileID3v1ViewModel);
        IEnumerable<AudiofileID3v1ViewModel> AudiofilesToAudiofilesID3v1ViewModel(IEnumerable<Audiofile> audiofiles, IEventAggregator eventAggregator);
        IEnumerable<Audiofile> AudiofilesID3v1ViewModelToAudiofiles(IEnumerable<AudiofileViewModel> audiofilesID3v1ViewModel);
    }
}