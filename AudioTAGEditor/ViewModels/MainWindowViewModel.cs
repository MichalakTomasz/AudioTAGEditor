using AudioTAGEditor.Models;
using AudioTAGEditor.Services;
using Commons;
using EventAggregator;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Unity;

namespace AudioTAGEditor.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        #region Constructor

        public MainWindowViewModel(
            [Dependency(nameof(ID3V1Service))]IID3Service id3v1Service,
            [Dependency(nameof(ID3V2Service))]IID3Service id3v2Service,
            IEventAggregator eventAggregator,
            IAudiofileConverter audioFileConverter,
            IFileService fileService,
            IHistoryService historyService,
            IAudiofileComparerService audiofileComparerService)
        {
            FilesFilter = ".mp3|.flac|.mpc|.ogg|.aac";
            ID3v1Service = id3v1Service;
            ID3v2Service = id3v2Service;
            EventAggregator = eventAggregator;
            AudiofileConverter = audioFileConverter;
            FileService = fileService;
            HistoryService = historyService;
            AudiofileComparerService = audiofileComparerService;
            eventAggregator.GetEvent<AudiofileMessageSentEvent>()
                .Subscribe(ExecuteMessage);
        }

        #endregion//Constructor

        #region Properties
        
        #region ToolBar

        private bool isCheckedID3v1;
        public bool IsCheckedID3v1
        {
            get { return isCheckedID3v1; }
            set { SetProperty(ref isCheckedID3v1, value); }
        }

        private bool isCheckedID3v2;
        public bool IsCheckedID3v2
        {
            get { return isCheckedID3v2; }
            set { SetProperty(ref isCheckedID3v2, value); }
        }

        #endregion//ToolBar

        #region TreeViewExplorer

        private string filesFilter;
        public string FilesFilter
        {
            get { return filesFilter; }
            set { SetProperty(ref filesFilter, value); }
        }

        private IEnumerable<string> filepathCollection;
        public IEnumerable<string> FilepathCollection
        {
            get { return filepathCollection; }
            set { SetProperty(ref filepathCollection, value); }
        }

        private string selectedPath;
        public string SelectedPath
        {
            get { return selectedPath; }
            set { SetProperty(ref selectedPath, value); }
        }

        #region DataGridBehaviors

        public IHistoryService HistoryService { get; }
        public IAudiofileComparerService AudiofileComparerService { get; }
        public IAudiofileConverter AudiofileConverter { get; }
        public IFileService FileService { get; }
        public IID3Service ID3v1Service { get; }
        public IID3Service ID3v2Service { get; }
        public IEventAggregator EventAggregator { get;}

        #endregion // DatagridEditBehaviors

        #endregion // TreeViewExplorer

        #region DataGridFiles

        private IEnumerable<AudiofileViewModel> audiofiles;
        public IEnumerable<AudiofileViewModel> Audiofiles
        {
            get { return audiofiles; }
            set { SetProperty(ref audiofiles, value); }
        }

        private IReadOnlyList<string> genres;
        public IReadOnlyList<string> Genres
        {
            get { return genres; }
            set { SetProperty(ref genres, value); }
        }

        private bool isEnabledDataGrid;
        public bool IsEnabledDataGrid
        {
            get { return isEnabledDataGrid; }
            set { SetProperty(ref isEnabledDataGrid, value); }
        }

        private bool allFilesChecked;
        public bool AllFilesChecked
        {
            get { return allFilesChecked; }
            set { SetProperty(ref allFilesChecked, value); }
        }

        private int historyCount;
        public int HistoryCount
        {
            get { return historyCount; }
            set { SetProperty(ref historyCount, value); }
        }

        private int historyPosition;
        public int HistoryPosition
        {
            get { return historyPosition; }
            set { SetProperty(ref historyPosition, value); }
        }

        #endregion//DataGridFiles

        #region StatusBar

        private string logMessage;
        public string LogMessage
        {
            get { return logMessage; }
            set { SetProperty(ref logMessage, value); }
        }

        #endregion // StatusBar

        #endregion // Properties

        #region Commands

        private ICommand expandNodeCommand;
        public ICommand ExpandNodeCommand
            => expandNodeCommand ?? (expandNodeCommand = 
            new DelegateCommand(ExpandCommandExecute));

        private ICommand checkAllFilesCommand;
        public ICommand CheckAllFilesCommand
            =>  checkAllFilesCommand ??  
            (checkAllFilesCommand = 
            new DelegateCommand(CheckAllFilesCommandExecute));

        private ICommand checkID3v1Command;
        public ICommand CheckID3v1Command
            => checkID3v1Command ??
            (checkID3v1Command = new DelegateCommand(
                CheckID3v1CommandExecute,
                CheckID3v1CommandCanExecute)
            .ObservesProperty(() => IsEnabledDataGrid));

        private ICommand checkID3v2Command;
        public ICommand CheckID3v2Command
            => checkID3v2Command ?? 
            (checkID3v2Command = 
            new DelegateCommand(CheckID3v2CommandExecute,
                CheckID3v2CommandCanExecute)
            .ObservesProperty(() => IsEnabledDataGrid));

        private ICommand exitCommand;
        public ICommand ExitCommand
            => exitCommand ?? 
            (exitCommand = new DelegateCommand(() => 
            App.Current.MainWindow.Close()));

        private ICommand undoCommand;
        public ICommand UndoCommand =>
            undoCommand ?? (undoCommand = 
            new DelegateCommand(
                UndoCommandExecute,
                UndoCommandCanExecute)
            .ObservesProperty(() => HistoryCount)
            .ObservesProperty(() => HistoryPosition));

        private ICommand redoCommand;
        public ICommand RedoCommand =>
            redoCommand ?? (redoCommand = 
            new DelegateCommand(
                RedoCommandExecute,
                RedoCommandCanExecute)
            .ObservesProperty(() => HistoryCount)
            .ObservesProperty(() => HistoryPosition));

        private ICommand historyConfirmCommand;
        public ICommand HistoryConfirmCommand =>
            historyConfirmCommand ?? (historyConfirmCommand = 
            new DelegateCommand(
                HistoryConfirmCommandExecute,
                HistoryConfirmCommandCanExecute)
            .ObservesProperty(() => HistoryCount)
            .ObservesProperty(() => HistoryPosition));

        private ICommand historyCancelCommand;
        public ICommand HistoryCancelCommand =>
            historyCancelCommand ?? (historyCancelCommand = 
            new DelegateCommand(
                HistoryCancelCommandExecute,
                HistoryCancelCommandCanExectute));
        
        #endregion//Commands

        #region Methods

        private void ExpandCommandExecute()
            => RefreshMainGrid();

        private void CheckAllFilesCommandExecute()
        {
            if (AllFilesChecked)
            {
                if (Audiofiles?.Count() > 0)
                    foreach (var item in Audiofiles)
                        item.IsChecked = true;
            }
            else
            {
                if (Audiofiles?.Count() > 0)
                    foreach (var item in Audiofiles)
                        item.IsChecked = false;
            }
        }

        private TagType ResolveTagToActivate()
        {
            var hasID3v2 = FilepathCollection
                .Any(f => ID3v2Service.HasTag(f));

            if (hasID3v2)
            {
                IsCheckedID3v2 = true;
                return TagType.ID3V2;
            }
            else
            {
                IsCheckedID3v1 = true;
                return TagType.ID3V1;
            }
        }

        private void CheckID3v1CommandExecute()
            => RefreshMainGrid(TagType.ID3V1);
        
        private void CheckID3v2CommandExecute()
            => RefreshMainGrid(TagType.ID3V2);

        private void RefreshGenres(TagType tagType)
        {
            switch (tagType)
            {
                case TagType.ID3V1:
                    Genres = ID3v1Service.GetGenres();
                    break;
                case TagType.ID3V2:
                    Genres = ID3v2Service.GetGenres();
                    break;
            }
        }

        private void RefreshMainGrid(TagType tagType = TagType.none)
        {
            ResetMainGrid();
            HistoryService.Clear();
            UpdateHistoryProperties();

            IsEnabledDataGrid = (FilepathCollection?.Count() > 0);
            if (!IsEnabledDataGrid) return;

            var localTagType = tagType;
            if (localTagType == TagType.none)
                localTagType = ResolveTagToActivate();

            SetTag(localTagType);

            RefreshGenres(localTagType);
            var audioFiles = new List<AudiofileViewModel>();
            switch (localTagType)
            {
                case TagType.ID3V1:
                    FilepathCollection.ToList().ForEach(file =>
                    {
                        var audioFile = ID3v1Service.GetTag(file);
                        var audioFileViewModel =
                        AudiofileConverter
                        .AudiofileToAudiofileID3v1ViewModel(audioFile, EventAggregator);
                        audioFiles.Add(audioFileViewModel);
                    }); 
                    break;
                case TagType.ID3V2:
                    FilepathCollection.ToList().ForEach(file =>
                    {
                        var audioFile = ID3v2Service.GetTag(file);
                        var audioFileViewModel =
                        AudiofileConverter
                        .AudiofileToAudiofileViewModel(audioFile, EventAggregator);
                        audioFiles.Add(audioFileViewModel);
                    });
                    break;
            }

            Audiofiles = audioFiles;
            AllFilesChecked = true;
            CheckAllFilesCommandExecute();
        }
        
        private void UpdateHistoryProperties()
        {
            HistoryCount = HistoryService.Count;
            HistoryPosition = HistoryService.Position;
        }

        private void SetTag(TagType tagType)
        {
            switch (tagType)
            {
                case TagType.ID3V1:
                    IsCheckedID3v1 = true;
                    break;
                case TagType.ID3V2:
                    IsCheckedID3v2 = true;
                    break;
            }
        }

        private void ResetMainGrid()
        {
            Audiofiles = null;
            AllFilesChecked = false;
            IsCheckedID3v1 = false;
            IsCheckedID3v2 = false;
        }

        private void ExecuteMessage(AudiofileMessage message)
        {
            if (!message.IsSelectedFile)
                AllFilesChecked = false;
            else
            {
                if (!Audiofiles.Any(a => !a.IsChecked))
                    AllFilesChecked = true;
            }
        }

        private void UndoCommandExecute() { }

        private bool UndoCommandCanExecute()
            => HistoryPosition > 0;

        private void RedoCommandExecute() { }

        private bool RedoCommandCanExecute()
            => HistoryCount > 0 && HistoryPosition < HistoryCount;

        private void HistoryConfirmCommandExecute() { }
        
        private bool CheckID3v1CommandCanExecute()
            => IsEnabledDataGrid;

        private bool CheckID3v2CommandCanExecute()
            => IsEnabledDataGrid;

        private bool HistoryConfirmCommandCanExecute()
            => HistoryCount > 0 && HistoryPosition < HistoryCount;

        void HistoryCancelCommandExecute()
        {
            HistoryService.ResetPosition();
            UpdateHistoryProperties();
        }

        private bool HistoryCancelCommandCanExectute()
            => HistoryCount > 0 && HistoryPosition < HistoryCount;

        #endregion // Methods
    }
}
