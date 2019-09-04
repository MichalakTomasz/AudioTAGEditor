using AudioTAGEditor.Models;
using AudioTAGEditor.Services;
using Prism.Commands;
using Prism.Mvvm;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Unity;

namespace AudioTAGEditor.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        #region Fields

        private readonly IID3Service id3V1Servece;
        private readonly IID3Service id3V2Service;

        #endregion//Fields

        #region Constructor

        public MainWindowViewModel(
            [Dependency(nameof(ID3V1Service))]IID3Service id3v1Servece,
            [Dependency(nameof(ID3V2Service))]IID3Service id3v2Service)
        {
            FilesFilter = ".mp3|.flac|.mpc|.ogg|.aac";
            id3V1Servece = id3v1Servece;
            id3V2Service = id3v2Service;
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

        private ObservableCollection<AudioFile> audioFiles
            = new ObservableCollection<AudioFile>();
        public ObservableCollection<AudioFile> AudioFiles
        {
            get { return audioFiles; }
            set { SetProperty(ref audioFiles, value); }
        }

        private bool isEnabledDataGrid;
        public bool IsEnabledDataGrid
        {
            get { return isEnabledDataGrid; }
            set { SetProperty(ref isEnabledDataGrid, value); }
        }

        private bool isSelectAllChecked;
        public bool IsSelectAllFilesChecked
        {
            get { return isSelectAllChecked; }
            set { SetProperty(ref isSelectAllChecked, value); }
        }

        private ObservableCollection<string> genres;
        public ObservableCollection<string> Genres
        {
            get { return genres; }
            set { SetProperty(ref genres, value); }
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

        private ICommand selectAllFilesCommand;
        public ICommand SelectAllFilesCommand
        {
            get
            {
                if (selectAllFilesCommand == null)
                    selectAllFilesCommand = new DelegateCommand(SelectAllFilesCommandExecute);
                return selectAllFilesCommand;
            }
        }

        private ICommand exitCommand;
        public ICommand ExitCommand
        {
            get
            {
                if (exitCommand == null)
                    exitCommand = new DelegateCommand(() => App.Current.MainWindow.Close());
                return exitCommand;
            }
        }

        #endregion//Commands

        #region Methods

        private void ExpandCommandExecute()
        {
            IsSelectAllFilesChecked = false;
            AudioFiles.Clear();
            IsEnabledDataGrid = (filePathCollection?.Count() > 0);
            if (IsEnabledDataGrid)
            {
                var selectedTag = ActivateTag(filePathCollection);
                AudioFile audioFile = null;
                foreach (var filePath in FilePathCollection)
                {
                    if (selectedTag == TagType.ID3V1)
                        audioFile = id3V1Servece.GetTag(filePath);
                    else
                        audioFile = id3V2Service.GetTag(filePath);
                    AudioFiles.Add(audioFile);
                }
                IsSelectAllFilesChecked = true;
            }  
        }

        private void SelectAllFilesCommandExecute()
        {
            if (IsSelectAllFilesChecked)
            {
                if (AudioFiles?.Count > 0)
                    foreach (var item in AudioFiles)
                        item.IsSelected = true;
            }
            else
            {
                if (AudioFiles?.Count > 0)
                    foreach (var item in AudioFiles)
                        item.IsSelected = false;
            }
        }

        private TagType ActivateTag(IEnumerable<string> filePathCollection)
        {
            var hasID3v2 = filePathCollection.Any(f => id3V2Service.HasTag(f));
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

        #endregion//Methods
    }
}
