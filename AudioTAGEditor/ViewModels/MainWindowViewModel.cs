using AudioTAGEditor.Models;
using AudioTAGEditor.Services;
using AutoMapper;
using Commons;
using EventAggregator;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using Unity;

namespace AudioTAGEditor.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        #region Fields

        private readonly IID3Service id3V1Service;
        private readonly IID3Service id3V2Service;
        private readonly IEventAggregator eventAggregator;
        private readonly IMapper mapper;

        #endregion//Fields

        #region Constructor

        public MainWindowViewModel(
            [Dependency(nameof(ID3V1Service))]IID3Service id3v1Servece,
            [Dependency(nameof(ID3V2Service))]IID3Service id3v2Service,
            IEventAggregator eventAggregator,
            IMapper mapper)
        {
            FilesFilter = ".mp3|.flac|.mpc|.ogg|.aac";
            id3V1Service = id3v1Servece;
            id3V2Service = id3v2Service;
            this.eventAggregator = eventAggregator;
            this.mapper = mapper;
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

        #endregion//TreeViewExplorer

        #region DataGreidFiles

        private IEnumerable<AudioFileViewModel> audioFiles;
        public IEnumerable<AudioFileViewModel> AudioFiles
        {
            get { return audioFiles; }
            set { SetProperty(ref audioFiles, value); }
        }

        private IDictionary<int, string> genres;
        public IDictionary<int, string> Genres
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
        {
            get
            {
                if (checkAllFilesCommand == null)
                    checkAllFilesCommand = new DelegateCommand(CheckAllFilesCommandExecute)
                        .ObservesProperty(() => AllFilesChecked);
                return checkAllFilesCommand;
            }
        }

        private ICommand checkID3v1Command;
        public ICommand CheckID3v1Command
        {
            get
            {
                if (checkID3v1Command == null)
                    checkID3v1Command = new DelegateCommand(CheckID3v1CommandExecute);
                return checkID3v1Command;
            }
        }

        private ICommand checkID3v2Command;
        public ICommand CheckID3v2Command
        {
            get
            {
                if (checkID3v2Command == null)
                    checkID3v2Command = new DelegateCommand(CheckID3v2CommandExecute);
                return checkID3v2Command;
            }
        }

        private ICommand exitCommand;
        public ICommand ExitCommand
        {
            get
            {
                if (exitCommand == null)
                    exitCommand = new DelegateCommand(() => 
                    App.Current.MainWindow.Close());
                return exitCommand;
            }
        }

        private ICommand cellEditEndingCommand;
        public ICommand CellEditEndingCommand
        {
            get
            {
                if (cellEditEndingCommand == null)
                    cellEditEndingCommand = 
                        new DelegateCommand<DataGridCellEditEndingEventArgs>(CellEditEndingCommandExecute);
                return cellEditEndingCommand;
            }
        }

        private void CellEditEndingCommandExecute(DataGridCellEditEndingEventArgs e)
        {
            if (e.EditAction == DataGridEditAction.Commit)
            {
               
            }
        }

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
            var hasID3v2 = FilePathCollection.Any(f => id3V2Service.HasTag(f));
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
                    Genres = id3V1Service.GetGenres();
                    break;
                case TagType.ID3V2:
                    Genres = id3V2Service.GetGenres();
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

            CheckTag(localTagType);

            RefreshGenres(localTagType);
            var audioFiles = new List<AudioFileViewModel>();
            switch (localTagType)
            {
                case TagType.ID3V1:
                    FilePathCollection.ToList().ForEach(file =>
                    {
                        var audioFile = id3V1Service.GetTag(file);
                        var audioFileViewModel =
                        mapper.Map(audioFile, new AudioFileViewModel(eventAggregator));
                        audioFiles.Add(audioFileViewModel);
                    }); 
                    break;
                case TagType.ID3V2:
                    FilePathCollection.ToList().ForEach(file =>
                    {
                        var audioFile = id3V2Service.GetTag(file);
                        var audioFileViewModel = 
                        mapper.Map(audioFile, new AudioFileViewModel(eventAggregator));
                        audioFiles.Add(audioFileViewModel);
                    });
                    break;
            }

            AudioFiles = audioFiles;
            AllFilesChecked = true;
            CheckAllFilesCommandExecute();
        }

        private void CheckTag(TagType tagType)
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

        #endregion//Methods
    }
}
