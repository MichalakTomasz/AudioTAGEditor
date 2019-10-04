using AudioTAGEditor.Models;
using AudioTAGEditor.Services;
using Commons;
using EventAggregator;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Unity;

namespace AudioTAGEditor.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        #region Fields

        private readonly IEventAggregator eventAggregator;

        #endregion//Fields

        #region Constructor

        public MainWindowViewModel(
            [Dependency(nameof(ID3V1Service))]IID3Service id3v1Service,
            [Dependency(nameof(ID3V2Service))]IID3Service id3v2Service,
            IEventAggregator eventAggregator,
            IAudioFileConverter audioFileConverter,
            IFileService fileService,
            IHistoryService historyService,
            IAudioFileComparerService audioFileComparerService)
        {
            FilesFilter = ".mp3|.flac|.mpc|.ogg|.aac";
            ID3v1Service = id3v1Service;
            ID3v2Service = id3v2Service;
            this.eventAggregator = eventAggregator;
            AudioFileConverter = audioFileConverter;
            FileService = fileService;
            HistoryService = historyService;
            AudioFileComparerService = audioFileComparerService;
            eventAggregator.GetEvent<AudioFileMessageSentEvent>()
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

        private IEnumerable<string> filePathCollection;
        public IEnumerable<string> FilePathCollection
        {
            get { return filePathCollection; }
            set { SetProperty(ref filePathCollection, value); }
        }

        private string selectedPath;
        public string SelectedPath
        {
            get { return selectedPath; }
            set { SetProperty(ref selectedPath, value); }
        }

        #region DataGridBehaviors

        public IHistoryService HistoryService { get; }

        public IAudioFileComparerService AudioFileComparerService { get; }

        public IAudioFileConverter AudioFileConverter { get; }

        public IFileService FileService { get; }

        public IID3Service ID3v1Service { get; }

        public IID3Service ID3v2Service { get; }

        #endregion // DatagridEditBehaviors

        #endregion // TreeViewExplorer

        #region DataGridFiles

        private IEnumerable<AudioFileViewModel> audioFiles;
        public IEnumerable<AudioFileViewModel> AudioFiles
        {
            get { return audioFiles; }
            set { SetProperty(ref audioFiles, value); }
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

        #endregion//Properties

        #region Commands

        private ICommand expandNodeCommand;
        public ICommand ExpandNodeCommand
        {
            get
            {
                if (expandNodeCommand == null)
                    expandNodeCommand = new DelegateCommand(ExpandCommandExecute);
                return expandNodeCommand;
            }
        }

        private ICommand checkAllFilesCommand;
        public ICommand CheckAllFilesCommand
            =>  checkAllFilesCommand ??  
            (checkAllFilesCommand = new DelegateCommand(CheckAllFilesCommandExecute));

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
            (checkID3v2Command = new DelegateCommand(CheckID3v2CommandExecute,
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

        #endregion//Commands

        #region Methods

        private void ExpandCommandExecute()
            => RefreshMainGrid();

        private void CheckAllFilesCommandExecute()
        {
            if (AllFilesChecked)
            {
                if (AudioFiles?.Count() > 0)
                    foreach (var item in AudioFiles)
                        item.IsChecked = true;
            }
            else
            {
                if (AudioFiles?.Count() > 0)
                    foreach (var item in AudioFiles)
                        item.IsChecked = false;
            }
        }

        private TagType ResolveTagToActivata()
        {
            var hasID3v2 = FilePathCollection.Any(f => ID3v2Service.HasTag(f));
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

            IsEnabledDataGrid = (FilePathCollection?.Count() > 0);
            if (!IsEnabledDataGrid) return;

            var localTagType = tagType;
            if (localTagType == TagType.none)
                localTagType = ResolveTagToActivata();

            SetTag(localTagType);

            RefreshGenres(localTagType);
            var audioFiles = new List<AudioFileViewModel>();
            switch (localTagType)
            {
                case TagType.ID3V1:
                    FilePathCollection.ToList().ForEach(file =>
                    {
                        var audioFile = ID3v1Service.GetTag(file);
                        var audioFileViewModel =
                        AudioFileConverter.AudioFileToAudioFileViewModel(audioFile, eventAggregator);
                        audioFiles.Add(audioFileViewModel);
                    }); 
                    break;
                case TagType.ID3V2:
                    FilePathCollection.ToList().ForEach(file =>
                    {
                        var audioFile = ID3v2Service.GetTag(file);
                        var audioFileViewModel =
                        AudioFileConverter
                        .AudioFileToAudioFileViewModel(audioFile, eventAggregator);
                        audioFiles.Add(audioFileViewModel);
                    });
                    break;
            }

            AudioFiles = audioFiles;
            AllFilesChecked = true;
            CheckAllFilesCommandExecute();
        }

        private void SetHistoryStepIntoMainGrid(HistoryObject historyObject)
        {
            var historyGridDataSource = new List<AudioFileViewModel>();

            AudioFiles.ToList().ForEach(i =>
            {
                var AudioFileFromHistory = 
                historyObject.AudioFiles
                .FirstOrDefault(f => f.ID == i.ID);

                if (AudioFileFromHistory != null)
                {
                    var audioFileViewModelFromHistory = AudioFileConverter
                    .AudioFileToAudioFileViewModel(
                        AudioFileFromHistory,
                        eventAggregator);

                    historyGridDataSource.Add(audioFileViewModelFromHistory);
                }
                else historyGridDataSource.Add(i);
            });

            AudioFiles = historyGridDataSource;
            UpdateHistoryProperties();
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
            AudioFiles = null;
            AllFilesChecked = false;
            IsCheckedID3v1 = false;
            IsCheckedID3v2 = false;
        }

        private void ExecuteMessage(AudioFileMessage message)
        {
            if (!message.IsSelectedFile)
                AllFilesChecked = false;
            else
            {
                if (!AudioFiles.Any(a => !a.IsChecked))
                    AllFilesChecked = true;
            }
        }

        void UndoCommandExecute() { }

        private bool UndoCommandCanExecute()
            => HistoryPosition > 0;

        void RedoCommandExecute() { }

        private bool RedoCommandCanExecute()
            => HistoryCount > 0 && HistoryPosition < HistoryCount;

        void HistoryConfirmCommandExecute()
        {

        }

        private bool CheckID3v1CommandCanExecute()
            => IsEnabledDataGrid;

        private bool CheckID3v2CommandCanExecute()
            => IsEnabledDataGrid;

        private bool HistoryConfirmCommandCanExecute()
            => HistoryCount > 0 && HistoryPosition < HistoryCount;

        #endregion//Methods
    }
}
