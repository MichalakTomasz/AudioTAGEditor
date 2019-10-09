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
        #region Fields

        private readonly IEventAggregator eventAggregator;

        #endregion//Fields

        #region Constructor

        public MainWindowViewModel(
            [Dependency(nameof(ID3V1Service))]IID3Service id3v1Service,
            [Dependency(nameof(ID3V2Service))]IID3Service id3v2Service,
            IEventAggregator eventAggregator,
            IAudiofileConverter audioFileConverter,
            IFileService fileService,
            IHistoryService historyService,
            IAudiofileComparerService audioFileComparerService)
        {
            FilesFilter = ".mp3|.flac|.mpc|.ogg|.aac";
            ID3v1Service = id3v1Service;
            ID3v2Service = id3v2Service;
            this.eventAggregator = eventAggregator;
            AudioFileConverter = audioFileConverter;
            FileService = fileService;
            HistoryService = historyService;
            AudioFileComparerService = audioFileComparerService;
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

        public IAudiofileComparerService AudioFileComparerService { get; }

        public IAudiofileConverter AudioFileConverter { get; }

        public IFileService FileService { get; }

        public IID3Service ID3v1Service { get; }

        public IID3Service ID3v2Service { get; }

        #endregion // DatagridEditBehaviors

        #endregion // TreeViewExplorer

        #region DataGridFiles

        private IEnumerable<AudiofileViewModel> audioFiles;
        public IEnumerable<AudiofileViewModel> AudioFiles
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

        private TagType ResolveTagToActivate()
        {
            var hasID3v2 = FilePathCollection
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

            IsEnabledDataGrid = (FilePathCollection?.Count() > 0);
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
                    FilePathCollection.ToList().ForEach(file =>
                    {
                        var audioFile = ID3v1Service.GetTag(file);
                        var audioFileViewModel =
                        AudioFileConverter
                        .AudioFileToAudioFileID3v1ViewModel(audioFile, eventAggregator);
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

        private void SetHistoryStepToMainGrid(HistoryStepType historyStepType)
        {
            var audioFiles = AudioFileConverter
                .AudioFilesViewModelToAudioFiles(AudioFiles);

            var resultHistoryObject = new HistoryObject();
            switch (historyStepType)
            {
                case HistoryStepType.Undo:
                    resultHistoryObject = HistoryService.Undo(audioFiles);
                    break;
                case HistoryStepType.Redo:
                    resultHistoryObject = HistoryService.Redo(audioFiles);
                    break;
                default:
                    break;
            }
            
            var audioFilesFromHistory = resultHistoryObject.Audiofiles;
            var tempAudioFileList = new List<AudiofileViewModel>();
            var selectedTag = GetTagTypeSelection();

            AudioFiles.ToList().ForEach(a =>
            {
                var fileToReplace = audioFilesFromHistory
                .FirstOrDefault(f => f.ID == a.ID);

                if (fileToReplace != null)
                {
                    switch (selectedTag)
                    {
                        case TagType.ID3V1:
                            var tempAudioFileID3v1ViewModel = AudioFileConverter
                            .AudioFileToAudioFileID3v1ViewModel(fileToReplace, eventAggregator);
                            tempAudioFileList.Add(tempAudioFileID3v1ViewModel);
                            break;
                        case TagType.ID3V2:
                            var tempAudioFileViewModel = AudioFileConverter
                            .AudioFileToAudioFileViewModel(fileToReplace, eventAggregator);
                            tempAudioFileList.Add(tempAudioFileViewModel);
                            break;
                    }
                }
                else
                    tempAudioFileList.Add(a);
            });

            AudioFiles = tempAudioFileList;
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

        private TagType GetTagTypeSelection()
        {
            if (IsCheckedID3v1)
                return TagType.ID3V1;

            if (IsCheckedID3v2)
                return TagType.ID3V2;

            return TagType.none;
        }

        private void ResetMainGrid()
        {
            AudioFiles = null;
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
                if (!AudioFiles.Any(a => !a.IsChecked))
                    AllFilesChecked = true;
            }
        }

        private void UndoCommandExecute()
            => SetHistoryStepToMainGrid(HistoryStepType.Undo);

        private bool UndoCommandCanExecute()
            => HistoryPosition > 0;

        private void RedoCommandExecute()
            => SetHistoryStepToMainGrid(HistoryStepType.Redo);

        private bool RedoCommandCanExecute()
            => HistoryCount > 0 && HistoryPosition < HistoryCount;

        private void HistoryConfirmCommandExecute()
        {
            if (!AudioFiles.Any(a => a.HasErrors))
            {
                
            }
        }

        private bool CheckID3v1CommandCanExecute()
            => IsEnabledDataGrid;

        private bool CheckID3v2CommandCanExecute()
            => IsEnabledDataGrid;

        private bool HistoryConfirmCommandCanExecute()
            => HistoryCount > 0 && HistoryPosition < HistoryCount;

        void HistoryCancelCommandExecute()
        {

        }

        private bool HistoryCancelCommandCanExectute()
        {
            return true;
        }

        #endregion//Methods
    }
}
