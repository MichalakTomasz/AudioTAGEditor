using AudioTAGEditor.Models;
using AudioTAGEditor.Services;
using Commons;
using EventAggregator;
using LibValidation;
using Prism.Commands;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Unity;

namespace AudioTAGEditor.ViewModels
{
    public class MainWindowViewModel : BindableBaseWithValidation
    {
        #region Constructor

        public MainWindowViewModel(
            [Dependency(nameof(ID3V1Service))]IID3Service id3v1Service,
            [Dependency(nameof(ID3V2Service))]IID3Service id3v2Service,
            IEventAggregator eventAggregator,
            IAudiofileConverter audioFileConverter,
            IFileService fileService,
            IHistoryService historyService,
            IAudiofileComparerService audiofileComparerService,
            ILogService logService,
            IFilenameEditService filenameEditService,
            IAudiofileCloneService audiofieleCloneService)
        {
            FilesFilter = ".mp3|.flac|.mpc|.ogg|.aac";
            EditFromID3Patterns = EditFromID3PatterntsList;
            ID3v1Service = id3v1Service;
            ID3v2Service = id3v2Service;
            EventAggregator = eventAggregator;
            AudiofileConverter = audioFileConverter;
            FileService = fileService;
            HistoryService = historyService;
            AudiofileComparerService = audiofileComparerService;
            LogService = logService;
            FilenameEditService = filenameEditService;
            this.audiofieleCloneService = audiofieleCloneService;
            eventAggregator.GetEvent<AudiofileMessageSentEvent>()
                .Subscribe(ExecuteMessage);
        }

        #endregion//Constructor

        #region Properties

        #region Ribbon

        #region Tab EditFilenames

        #region Cut

        private bool isEnabledCutGroup;
        public bool IsEnabledCutGroup
        {
            get { return isEnabledCutGroup; }
            set { SetProperty(ref isEnabledCutGroup, value); }
        }

        private bool isCheckedCutSpace;
        public bool IsCheckedCutSpace
        {
            get { return isCheckedCutSpace; }
            set { SetProperty(ref isCheckedCutSpace, value); }
        }

        private bool isCheckedCutDot;
        public bool IsCheckedCutDot
        {
            get { return isCheckedCutDot; }
            set { SetProperty(ref isCheckedCutDot, value); }
        }

        private bool isCheckedCutUnderscore;
        public bool IsCheckedCutUnderscore
        {
            get { return isCheckedCutUnderscore; }
            set { SetProperty(ref isCheckedCutUnderscore, value); }
        }

        private bool isCheckedCutDash;
        public bool IsCheckedCutDash
        {
            get { return isCheckedCutDash; }
            set { SetProperty(ref isCheckedCutDash, value); }
        }

        #endregion // Cut

        #region Replace to space

        private bool isEnabledReplaceToSpaceGroup;
        public bool IsEnabledReplaceToSpaceGroup
        {
            get { return isEnabledReplaceToSpaceGroup; }
            set { SetProperty(ref isEnabledReplaceToSpaceGroup, value); }
        }

        private bool isCheckedReplaceDotToSpace;
        public bool IsCheckedReplaceDotToSpace
        {
            get { return isCheckedReplaceDotToSpace; }
            set { SetProperty(ref isCheckedReplaceDotToSpace, value); }
        }

        private bool isCheckedReplaceUnderscoreToSpace;
        public bool IsCheckedReplaceUnderscoreToSpace
        {
            get { return isCheckedReplaceUnderscoreToSpace; }
            set { SetProperty(ref isCheckedReplaceUnderscoreToSpace, value); }
        }

        private bool isCheckedReplaceDashToSpace;
        public bool IsCheckedReplaceDashToSpace
        {
            get { return isCheckedReplaceDashToSpace; }
            set { SetProperty(ref isCheckedReplaceDashToSpace, value); }
        }

        #endregion // Replace ot space

        #region Replace from space

        private bool isEnabledReplaceFromSpaceGroup;
        public bool IsEnabledReplaceFromSpaceGroup
        {
            get { return isEnabledReplaceFromSpaceGroup; }
            set { SetProperty(ref isEnabledReplaceFromSpaceGroup, value); }
        }

        private bool isCheckedReplaceSpaceToDot;
        public bool IsCheckedReplaceSpaceToDot
        {
            get { return isCheckedReplaceSpaceToDot; }
            set { SetProperty(ref isCheckedReplaceSpaceToDot, value); }
        }

        private bool isCheckedReplaceSpaceToUnderscore;
        public bool IsCheckedReplaceSpaceToUnderscore
        {
            get { return isCheckedReplaceSpaceToUnderscore; }
            set { SetProperty(ref isCheckedReplaceSpaceToUnderscore, value); }
        }

        private bool isCheckedReplaceSpaceToDash;
        public bool IsCheckedReplaceSpaceToDash
        {
            get { return isCheckedReplaceSpaceToDash; }
            set { SetProperty(ref isCheckedReplaceSpaceToDash, value); }
        }

        #endregion // Replace from space

        #region Change

        private bool isEnabledChangeGroup;
        public bool IsEnabledChangeGroup
        {
            get { return isEnabledChangeGroup; }
            set { SetProperty(ref isEnabledChangeGroup, value); }
        }

        private bool isCheckedChangeFirstCapitalLetter;
        public bool IsCheckedChangeFirstCapitalLetter
        {
            get { return isCheckedChangeFirstCapitalLetter; }
            set { SetProperty(ref isCheckedChangeFirstCapitalLetter, value); }
        }

        private bool isCheckedChangeAllFirstCapitalLetters;
        public bool IsCheckedChangeAllFirstCapitalLetters
        {
            get { return isCheckedChangeAllFirstCapitalLetters; }
            set { SetProperty(ref isCheckedChangeAllFirstCapitalLetters, value); }
        }

        private bool isCheckedChangeUpperCase;
        public bool IsCheckedChangeUpperCase
        {
            get { return isCheckedChangeUpperCase; }
            set { SetProperty(ref isCheckedChangeUpperCase, value); }
        }

        private bool isCheckedChangeLowerCase;
        public bool IsCheckedChangeLowerCase
        {
            get { return isCheckedChangeLowerCase; }
            set { SetProperty(ref isCheckedChangeLowerCase, value); }
        }

        #endregion // Change

        #region Insert from position

        private bool isEnabledInsertFromPositionGroup;
        public bool IsEnabledInsertFromPositionGroup
        {
            get { return isEnabledInsertFromPositionGroup; }
            set { SetProperty(ref isEnabledInsertFromPositionGroup, value); }
        }

        private int? insertFromPositionPosition;
        [Range(1, 255)]
        public int? InsertFromPositionPosition
        {
            get { return insertFromPositionPosition; }
            set { SetProperty(ref insertFromPositionPosition, value); }
        }

        private string insertFromPositionText;
        [RegularExpression("[^<>:\"\\|/?*]+")]
        public string InsertFromPositionText
        {
            get { return insertFromPositionText; }
            set { SetProperty(ref insertFromPositionText, value); }
        }

        #endregion // Insert from position

        #region Cut from position

        private bool isEnabledCutFromPositionGroup;
        public bool IsEnabledCutFromPositionGroup
        {
            get { return isEnabledCutFromPositionGroup; }
            set { SetProperty(ref isEnabledCutFromPositionGroup, value); }
        }

        private int? cutFromPositionPosition;
        [Range(1, 255)]
        public int? CutFromPositionPosition
        {
            get { return cutFromPositionPosition; }
            set { SetProperty(ref cutFromPositionPosition, value); }
        }

        private int? cutFromPositionCount;
        [Range(1, 255)]
        public int? CutFromPositionCount
        {
            get { return cutFromPositionCount; }
            set { SetProperty(ref cutFromPositionCount, value); }
        }

        #endregion // Cut from position

        #region Cut text

        private bool isEnabledCutTextGroup;
        public bool IsEnabledCutTextGroup
        {
            get { return isEnabledCutTextGroup; }
            set { SetProperty(ref isEnabledCutTextGroup, value); }
        }

        private string cutTextText;
        public string CutTextText
        {
            get { return cutTextText; }
            set { SetProperty(ref cutTextText, value); }
        }

        #endregion // Cut text

        #region Replace text

        private bool isEnabledReplaceTextGroup;
        public bool IsEnabledReplaceTextGroup
        {
            get { return isEnabledReplaceTextGroup; }
            set { SetProperty(ref isEnabledReplaceTextGroup, value); }
        }

        private string replaceTextOldText;
        public string ReplaceTextOldText
        {
            get { return replaceTextOldText; }
            set { SetProperty(ref replaceTextOldText, value); }
        }

        private string replaceTextNewText;
        [RegularExpression("[^<>:\"\\|/?*]+")]
        public string ReplaceTextNewText
        {
            get { return replaceTextNewText; }
            set { SetProperty(ref replaceTextNewText, value); }
        }

        #endregion // Replace text

        #region Insert numbering

        private bool isEnabledInsertNumberingGroup;
        public bool IsEnabledInsertNumberingGroup
        {
            get { return isEnabledInsertNumberingGroup; }
            set { SetProperty(ref isEnabledInsertNumberingGroup, value); }
        }

        private int? insertNumberingPosition;
        [Range(1, 255)]
        public int? InsertNumberingPosition
        {
            get { return insertNumberingPosition; }
            set { SetProperty(ref insertNumberingPosition, value); }
        }

        #endregion // Insert numbering

        #region Change from ID3

        private bool isEnabledChangeFromID3Group;
        public bool IsEnabledChangeFromID3Group
        {
            get { return isEnabledChangeFromID3Group; }
            set { SetProperty(ref isEnabledChangeFromID3Group, value); }
        }

        private string changeFromID3Pattern;
        [RegularExpression("[^:\"\\|/?*]+")]
        public string ChangeFromID3Pattern
        {
            get { return changeFromID3Pattern; }
            set { SetProperty(ref changeFromID3Pattern, value); }
        }

        private IEnumerable<string> editFromID3Patterns;
        public IEnumerable<string> EditFromID3Patterns
        {
            get { return editFromID3Patterns; }
            set { SetProperty(ref editFromID3Patterns, value); }
        }

        #endregion // Change form ID3

        #region Execute

        private bool isEnabledExecuteEditFilenamesGroup;
        public bool IsEnabledExecuteEditFilenamesGroup
        {
            get { return isEnabledExecuteEditFilenamesGroup; }
            set { SetProperty(ref isEnabledExecuteEditFilenamesGroup, value); }
        }

        #endregion // Execute

        #endregion // Tab EditFilenames
        
        #endregion // Ribbon

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

        private ExplorerTreeView.ExplorerTreeView explorerTreeView;
        public ExplorerTreeView.ExplorerTreeView ExplorerTreeView
        {
            get { return explorerTreeView; }
            set { SetProperty(ref explorerTreeView, value); }
        }

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
        public ILogService LogService { get; }
        public IFilenameEditService FilenameEditService { get; }
        public IAudiofileConverter AudiofileConverter { get; }
        public IFileService FileService { get; }
        public IID3Service ID3v1Service { get; }
        public IID3Service ID3v2Service { get; }
        public IEventAggregator EventAggregator { get; }

        #endregion // DatagridEditBehaviors

        #endregion // TreeViewExplorer

        #region DataGrid

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

        #endregion // DataGrid

        #region StatusBar

        private string logMessage;
        public string LogMessage
        {
            get { return logMessage; }
            set { SetProperty(ref logMessage, value); }
        }

        private LogMessageStatusType logMessageStatusType;
        public LogMessageStatusType LogMessageStatusType
        {
            get { return logMessageStatusType; }
            set { SetProperty(ref logMessageStatusType, value); }
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
            new DelegateCommand<ExplorerTreeView.ExplorerTreeView>(
                HistoryConfirmCommandExecute,
                HistoryConfirmCommandCanExecute)
            .ObservesProperty(() => HistoryCount)
            .ObservesProperty(() => HistoryPosition));

        private ICommand historyCancelCommand;
        public ICommand HistoryCancelCommand =>
            historyCancelCommand ?? (historyCancelCommand = 
            new DelegateCommand(
                HistoryCancelCommandExecute,
                HistoryCancelCommandCanExectute)
            .ObservesProperty(() => HistoryCount)
            .ObservesProperty(() => HistoryPosition));

        private ICommand executeFilenamesEditCommand;
        private readonly IAudiofileCloneService audiofieleCloneService;

        public ICommand ExecuteFilenamesEditCommand =>
            executeFilenamesEditCommand ?? (executeFilenamesEditCommand = 
            new DelegateCommand<ExplorerTreeView.ExplorerTreeView>(
                ExecuteExecuteFilenameEditCommand, 
                CanExecuteExecuteFilenameEditCommand));
        
        private ICommand cellBeginingEditCommand;
        public ICommand CellBeginingEditCommand =>
            cellBeginingEditCommand ?? (cellBeginingEditCommand = 
            new DelegateCommand<DataGridBeginningEditEventArgs>(ExecuteCellBeginingEditCommand));

        private ICommand cellEditEndingCommandExecute;
        public ICommand CellEditEndingCommandExecute =>
            cellEditEndingCommandExecute ?? (cellEditEndingCommandExecute = 
            new DelegateCommand<DataGridCellEditEndingEventArgs>(
                ExecuteCellEditEndingCommandExecute));

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
            RefreshEnabledEditFilenamesGroups();
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

        private TagType GetTagTypeSelection()
        {
            if (IsCheckedID3v1)
                return TagType.ID3V1;

            if (IsCheckedID3v2)
                return TagType.ID3V2;

            return TagType.none;
        }

        private void UpdateTag(Audiofile audiofile, string path)
        {
            var filepath = $"{path}{audiofile.Filename}";
            switch (audiofile.TagType)
            {
                case TagType.ID3V1:
                    ID3v1Service
                        .UpdateTag(audiofile, filepath, TagVersion.ID3V11);
                    break;
                case TagType.ID3V2:
                    ID3v2Service
                        .UpdateTag(audiofile, filepath, TagVersion.ID3V24);
                    break;
            }
        }

        private void SetHistoryStepToMainGrid(HistoryStepType historyStepType)
        {
            var audioFiles = AudiofileConverter
                .AudiofilesViewModelToAudiofiles(Audiofiles);

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

            Audiofiles.ToList().ForEach(a =>
            {
                var fileToReplace = audioFilesFromHistory
                .FirstOrDefault(f => f.ID == a.ID);

                if (fileToReplace != null)
                {
                    switch (selectedTag)
                    {
                        case TagType.ID3V1:
                            var tempAudioFileID3v1ViewModel = AudiofileConverter
                            .AudiofileToAudiofileID3v1ViewModel(fileToReplace, EventAggregator);
                            tempAudioFileID3v1ViewModel.IsChecked = a.IsChecked;
                            tempAudioFileList.Add(tempAudioFileID3v1ViewModel);
                            break;
                        case TagType.ID3V2:
                            var tempAudioFileViewModel = AudiofileConverter
                            .AudiofileToAudiofileViewModel(fileToReplace, EventAggregator);
                            tempAudioFileViewModel.IsChecked = a.IsChecked;
                            tempAudioFileList.Add(tempAudioFileViewModel);
                            break;
                    }
                }
                else
                    tempAudioFileList.Add(a);
            });

            Audiofiles = tempAudioFileList;
            UpdateHistoryProperties();
        }

        private void UndoCommandExecute()
            => SetHistoryStepToMainGrid(HistoryStepType.Undo);

        private bool UndoCommandCanExecute()
            => HistoryPosition > 0;

        private void RedoCommandExecute() 
            => SetHistoryStepToMainGrid(HistoryStepType.Redo);

        private bool RedoCommandCanExecute()
            => HistoryCount > 0 && HistoryPosition < HistoryCount;

        private void HistoryConfirmCommandExecute(
            ExplorerTreeView.ExplorerTreeView explorerTreeView) 
        {
            if (!Audiofiles.Any(a => a.HasErrors))
            {
                List<Audiofile> audiofiles = null;
                var selectedTag = GetTagTypeSelection();
                switch (selectedTag)
                {
                    case TagType.ID3V1:
                        audiofiles = AudiofileConverter
                            .AudiofilesID3v1ViewModelToAudiofiles(
                            Audiofiles)
                            .ToList();
                        break;
                    case TagType.ID3V2:
                        audiofiles = AudiofileConverter
                            .AudiofilesViewModelToAudiofiles(Audiofiles)
                            .ToList();
                        break;
                }

                var changedAudiofiles = new List<Audiofile>();
                var changedFilenames = new Dictionary<Guid, string>();
                audiofiles.ForEach(a =>
                {
                    var hasChanges = HistoryService
                    .HasChangesSinceCurrentHistoryPosition(a.ID);
                    if (hasChanges)
                    {
                        var currentFilename = 
                        HistoryService.GetCurrentFilename(a.ID);
                        if (currentFilename != a.Filename)
                        {
                            var oldFullFilename = $"{SelectedPath}{currentFilename}";
                            FileService.Rename(oldFullFilename, a.Filename);
                            changedFilenames.Add(a.ID, a.Filename);
                        }

                        var hasTagChanges = HistoryService
                        .HasTagChangesSienceCurrentPosition(a);
                        if (hasTagChanges)
                            UpdateTag(a, SelectedPath);
                    }
                    changedAudiofiles.Add(a);
                });

                HistoryService.Add(
                    changedAudiofiles,
                    ChangeActionType.Mixed,
                    SelectedPath,
                    changedFilenames);

                UpdateHistoryProperties();
                explorerTreeView.Refresh();

                MessageBox.Show(
                    "History was restored successlfully.",
                    "Informarion",
                     MessageBoxButton.OK,
                     MessageBoxImage.Information);

                var log = LogService.Add(
                    LogMessageStatusType.Information,
                    "History step was applied.");
                LogMessageStatusType = log.LogMessageStatusType;
                LogMessage = log.Message;
            }
        }
        
        private bool CheckID3v1CommandCanExecute()
            => IsEnabledDataGrid;

        private bool CheckID3v2CommandCanExecute()
            => IsEnabledDataGrid;

        private bool HistoryConfirmCommandCanExecute(
            ExplorerTreeView.ExplorerTreeView explorerTreeView)
            => HistoryCount > 0 && HistoryPosition < HistoryCount;

        private void HistoryCancelCommandExecute()
        {
            var historyObject = HistoryService.GoToLast();
            if (historyObject != null)
            {
                var audioFiles = historyObject.Audiofiles;
                var audiofilesViewModel = 
                    ConvertAudiofilesToAudiofilesViewModel(audioFiles);

                SetChangesToMainGrid(audiofilesViewModel);
            }
            
            UpdateHistoryProperties();
        }

        private bool HistoryCancelCommandCanExectute()
            => HistoryCount > 0 && HistoryPosition < HistoryCount;

        private IEnumerable<Audiofile> ConvertAudioFilesViewModelsToAudioFiles(
            IEnumerable<AudiofileViewModel> audiofileViewModels)
        {
            var selectedTag = ResolveTagToActivate();
            switch (selectedTag)
            {
                case TagType.ID3V1:
                    return AudiofileConverter
                        .AudiofilesID3v1ViewModelToAudiofiles(audiofileViewModels);
                case TagType.ID3V2:
                    return AudiofileConverter
                        .AudiofilesViewModelToAudiofiles(audiofileViewModels);
                default: 
                    return null;
            }
        }

        private IEnumerable<AudiofileViewModel> ConvertAudiofilesToAudiofilesViewModel(
            IEnumerable<Audiofile> audiofiles)
        {
            var selectedTag = ResolveTagToActivate();
            switch (selectedTag)
            {
                case TagType.ID3V1:
                    return AudiofileConverter
                        .AudiofilesToAudiofilesID3v1ViewModel(audiofiles, EventAggregator);
                case TagType.ID3V2:
                    return AudiofileConverter
                        .AudiofilesToAudiofilesViewModel(audiofiles, EventAggregator);
                default:
                    return null;
            }
        }

        private IEnumerable<AudiofileViewModel> SetChangesToMainGrid(
            IEnumerable<AudiofileViewModel> audiofilesViewModel)
        {
            var resultCollection = new List<AudiofileViewModel>();
            Audiofiles.ToList().ForEach(a =>
            {
                var tempAudiofileViewModel = audiofilesViewModel
                .FirstOrDefault(e => e.ID == a.ID);
                if (tempAudiofileViewModel != null)
                {
                    tempAudiofileViewModel.IsChecked = a.IsChecked;
                    resultCollection.Add(tempAudiofileViewModel);
                }
                else
                    resultCollection.Add(a);
            });

            return resultCollection;
        }

        private void RenameFiles(
            IEnumerable<Audiofile> oldAudiofiles, 
            IEnumerable<Audiofile> newAudiofiles)
        {
            var mergedAudiofiles = oldAudiofiles
                .Zip(newAudiofiles, (o, n) => 
                new { oldFilename = o.Filename, newFilename = n.Filename });

            mergedAudiofiles.ToList()
                .ForEach(m => 
                FileService.Rename(
                    $"{SelectedPath}{m.oldFilename}", 
                    m.newFilename));
        }

        void ExecuteExecuteFilenameEditCommand(
            ExplorerTreeView.ExplorerTreeView explorerTreeView)
        {
            if (Audiofiles.Any(a => a.HasErrors))
                return;

            var checkedAudioFilesViewModel = Audiofiles.Where(a => a.IsChecked);
            var checkedAudiofiles = 
                ConvertAudioFilesViewModelsToAudioFiles(checkedAudioFilesViewModel);
            var editedAudiofiles = audiofieleCloneService.Clone(checkedAudiofiles);

            #region Insert numbering

            if (InsertNumberingPosition != null)
            {
                var audiofiles =
                    ConvertAudioFilesViewModelsToAudioFiles(Audiofiles);
                var audiofilesWithNumbering = FilenameEditService.InsertNumbering(
                    audiofiles, InsertNumberingPosition.Value);

                InsertNumberingPosition = null;

                RenameFiles(audiofiles, audiofilesWithNumbering);
                explorerTreeView.Refresh();
                var newAudioFilesViewModel = ConvertAudiofilesToAudiofilesViewModel(audiofilesWithNumbering);
                Audiofiles = SetChangesToMainGrid(newAudioFilesViewModel);
                HistoryService.Add(audiofiles, ChangeActionType.Filename, SelectedPath);
                UpdateHistoryProperties();
            }

            #endregion // Insert numbering

            #region Cut

            if (IsCheckedCutSpace) editedAudiofiles =
                    FilenameEditService.CutSpace(editedAudiofiles);

            if (IsCheckedCutDot) editedAudiofiles =
                    FilenameEditService.CutDot(editedAudiofiles);

            if (IsCheckedCutUnderscore) editedAudiofiles =
                    FilenameEditService.CutUnderscore(editedAudiofiles);

            if (IsCheckedCutDash) editedAudiofiles =
                    FilenameEditService.CutDash(editedAudiofiles);

            #endregion // Cut

            #region Replace to space

            if (IsCheckedReplaceDotToSpace) editedAudiofiles =
                    FilenameEditService.ReplaceDotToSpace(editedAudiofiles);

            if (IsCheckedReplaceUnderscoreToSpace) editedAudiofiles =
                    FilenameEditService.ReplaceUnderscoreToSpace(editedAudiofiles);

            if (IsCheckedReplaceDashToSpace) editedAudiofiles =
                    FilenameEditService.ReplaceDashToSpace(editedAudiofiles);

            #endregion // Replace to spece

            #region Replace from space

            if (IsCheckedReplaceSpaceToDot) editedAudiofiles =
                    FilenameEditService.ReplaceSpaceToDot(editedAudiofiles);

            if (IsCheckedReplaceSpaceToUnderscore) editedAudiofiles =
                    FilenameEditService.ReplaceSpaceToUnderscore(editedAudiofiles);

            if (IsCheckedReplaceSpaceToDash) editedAudiofiles =
                    FilenameEditService.ReplaceSpaceToDash(editedAudiofiles);


            #endregion // Replace from space

            #region Change

            if (IsCheckedChangeFirstCapitalLetter) editedAudiofiles =
                    FilenameEditService.FirstCapitalLetter(editedAudiofiles);

            if (IsCheckedChangeAllFirstCapitalLetters) editedAudiofiles =
                    FilenameEditService.AllFirstCapitalLetters(editedAudiofiles);

            if (IsCheckedChangeUpperCase) editedAudiofiles =
                    FilenameEditService.UpperCase(editedAudiofiles);

            if (IsCheckedChangeLowerCase) editedAudiofiles =
                    FilenameEditService.LowerCase(editedAudiofiles);

            #endregion  // Change

            #region Insert from position

            if (InsertFromPositionPosition != null &&
                !string.IsNullOrWhiteSpace(InsertFromPositionText))
            {
                var audioFiles = ConvertAudioFilesViewModelsToAudioFiles(Audiofiles);
                editedAudiofiles = FilenameEditService.InsertTextFromPosition(
                    editedAudiofiles, InsertFromPositionPosition.Value, InsertFromPositionText);

                InsertFromPositionPosition = null;
                InsertFromPositionText = null;
            }

            #endregion // Insert from position

            #region Cut from position

            if (CutFromPositionPosition != null && CutFromPositionCount != null)
            {
                editedAudiofiles = FilenameEditService.CutText(
                    editedAudiofiles, CutFromPositionPosition.Value,
                    CutFromPositionCount.Value);

                CutFromPositionPosition = null;
                CutFromPositionCount = null;
            }

            #endregion // Cut from position

            #region Cut text

            if (!string.IsNullOrWhiteSpace(CutTextText))
            {
                editedAudiofiles = FilenameEditService.CutText(editedAudiofiles, CutTextText);
                CutTextText = null;
            }

            #endregion // Cut text

            #region Replace text

            if (!string.IsNullOrWhiteSpace(ReplaceTextOldText) &&
                !string.IsNullOrWhiteSpace(ReplaceTextNewText))
            {
                editedAudiofiles = FilenameEditService.ReplaceText(
                    editedAudiofiles, ReplaceTextNewText, ReplaceTextNewText);

                ReplaceTextOldText = null;
                ReplaceTextNewText = null;
            }

            #endregion // Replace text
            
            #region Change from ID3

            if (!string.IsNullOrWhiteSpace(ChangeFromID3Pattern))
            {
                editedAudiofiles = FilenameEditService.ChangeByPattern(
                    editedAudiofiles, ChangeFromID3Pattern);

                ChangeFromID3Pattern = null;
            }

            #endregion // Change from ID3

            #region Summary

            if (AudiofileComparerService
                .AreTheSame(checkedAudiofiles, editedAudiofiles))
                return;

            RenameFiles(checkedAudiofiles, editedAudiofiles);
            explorerTreeView.Refresh();

            var checkedEditedAudiofilesViewModel =
                ConvertAudiofilesToAudiofilesViewModel(editedAudiofiles);
            HistoryService.Add(checkedAudiofiles, ChangeActionType.Filename, SelectedPath);
            Audiofiles = SetChangesToMainGrid(checkedEditedAudiofilesViewModel);
            UpdateHistoryProperties();

            #endregion // Summary
        }

        bool CanExecuteExecuteFilenameEditCommand(
            ExplorerTreeView.ExplorerTreeView explorerTreeView)
        {
            return true;
        }

        private void RefreshEnabledEditFilenamesGroups()
        {
            if (Audiofiles?.Count() > 0)
            {
                IsEnabledCutGroup = true;
                IsEnabledReplaceToSpaceGroup = true;
                IsEnabledReplaceFromSpaceGroup = true;
                IsEnabledChangeGroup = true;
                IsEnabledInsertFromPositionGroup = true;
                IsEnabledCutFromPositionGroup = true;
                IsEnabledCutTextGroup = true;
                IsEnabledReplaceTextGroup = true;
                IsEnabledInsertNumberingGroup = true;
                IsEnabledChangeFromID3Group = true;
                IsEnabledExecuteEditFilenamesGroup = true;
            }
            else
            {
                IsEnabledCutGroup = false;
                IsEnabledReplaceToSpaceGroup = false;
                IsEnabledReplaceFromSpaceGroup = false;
                IsEnabledChangeGroup = false;
                IsEnabledInsertFromPositionGroup = false;
                IsEnabledCutFromPositionGroup = false;
                IsEnabledCutTextGroup = false;
                IsEnabledReplaceTextGroup = false;
                IsEnabledInsertNumberingGroup = false;
                IsEnabledChangeFromID3Group = false;
                IsEnabledExecuteEditFilenamesGroup = false;
            }
        }

        #region Grid cell edit

        void ExecuteCellBeginingEditCommand(DataGridBeginningEditEventArgs e)
        {
            if (e.Column.Header is CheckBox)
                return;

            var audioFileViewModel = e.Row.DataContext as AudiofileViewModel;
            var audioFile = AudiofileConverter.AudiofileViewModelToAudiofile(audioFileViewModel);

            audioFileBeforeEdit = audioFile;
        }

        void ExecuteCellEditEndingCommandExecute(DataGridCellEditEndingEventArgs e)
        {
            if (e.EditAction == DataGridEditAction.Commit && !(e.Column.Header is CheckBox))
            {
                var audiofileViewModel = e.EditingElement.DataContext as AudiofileViewModel;
                var filename = $"{SelectedPath}{audiofileViewModel.Filename}";


                if (!audiofileViewModel.HasErrors)
                {
                    var newAudioFile = AudiofileConverter
                        .AudiofileViewModelToAudiofile(audiofileViewModel);
                    var audioFileFullPath = $"{SelectedPath}{newAudioFile.Filename}";
                    LogItem log;

                    switch (e.Column.Header)
                    {
                        case "File Name":
                            var oldFilename = audioFileBeforeEdit.Filename;
                            var exist = FileService.Exist(filename);
                            if (exist)
                            {
                                log = LogService.Add(
                                    LogMessageStatusType.Error, 
                                    "This folder contains file with the same name.");
                                this.LogMessage = log.Message;
                                this.LogMessageStatusType = log.LogMessageStatusType;
                                return;
                            }

                            if (oldFilename == newAudioFile.Filename)
                                return;

                            var oldFilePath = $"{SelectedPath}{oldFilename}";
                            FileService.Rename(oldFilePath, newAudioFile.Filename);
                            HistoryService.Add(
                                audioFileBeforeEdit,
                                ChangeActionType.Filename,
                                SelectedPath,
                                newAudioFile.Filename);

                            UpdateHistoryProperties();
                            ExplorerTreeView.Refresh();

                            log = LogService.Add(
                                LogMessageStatusType.Information, 
                                "Filename changed.");

                            LogMessageStatusType = log.LogMessageStatusType;
                            LogMessage = log.Message;
                            break;
                        default:
                            if (AudiofileComparerService.AreTheSame(
                                audioFileBeforeEdit, 
                                newAudioFile))
                                return;

                            if (IsCheckedID3v1)
                            {
                                ID3v1Service.UpdateTag(
                                    newAudioFile, 
                                    audioFileFullPath, 
                                    TagVersion.ID3V11);
                                HistoryService.Add(
                                    audioFileBeforeEdit, 
                                    ChangeActionType.ID3v1, 
                                    SelectedPath);

                                UpdateHistoryProperties();

                                log = LogService.Add(
                                    LogMessageStatusType.Information,
                                    "ID3v1 Tag was changed.");
                                LogMessageStatusType = log.LogMessageStatusType;
                                LogMessage = log.Message;
                            }

                            if (IsCheckedID3v2)
                            {
                                ID3v2Service.UpdateTag(
                                    newAudioFile, 
                                    audioFileFullPath, 
                                    TagVersion.ID3V20);
                                HistoryService.Add(
                                    audioFileBeforeEdit, 
                                    ChangeActionType.ID3v2, 
                                    SelectedPath);

                                UpdateHistoryProperties();

                                log = LogService.Add(
                                    LogMessageStatusType.Information, 
                                    "ID3v2 Tag was changed.");
                                LogMessageStatusType = log.LogMessageStatusType;
                                LogMessage = log.Message;
                            }
                            break;
                    }
                }
            }
        }

        #endregion // Grid cell edit

        #endregion // Methods

        #region Fields

        private Audiofile audioFileBeforeEdit;
        private IEnumerable<string> EditFromID3PatterntsList =
            new List<string>
            {
                "<no>. <title>",
                "<no> <title>",
                "<no> <artist> - <album> - <title>",
                "<artist>-<album>-<title>"
            };

        #endregion // Fields
    }
}
