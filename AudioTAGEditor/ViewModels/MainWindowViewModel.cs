using AudioTAGEditor.Models;
using AudioTAGEditor.Services;
using ExplorerTreeView;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        private ObservableCollection<AudioFile> audioFiles = new ObservableCollection<AudioFile>();
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
        public bool IsSelectAllChecked
        {
            get { return isSelectAllChecked; }
            set { SetProperty(ref isSelectAllChecked, value); }
        }

        private TagType tagType;
        public TagType TAGType
        {
            get { return tagType; }
            set { SetProperty(ref tagType, value); }
        }

        private ObservableCollection<string> genres;
        public ObservableCollection<string> Genres
        {
            get { return genres; }
            set { SetProperty(ref genres, value); }
        }

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

        private ICommand selectAllCommand;
        public ICommand SelectAllCommand
        {
            get
            {
                if (selectAllCommand == null)
                    selectAllCommand = new DelegateCommand(SelectAllCommandExecute);
                return selectAllCommand;
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
            IsSelectAllChecked = false;
            AudioFiles.Clear();
            IsEnabledDataGrid = (filePathCollection?.Count() > 0);
            if (IsEnabledDataGrid)
            {
                foreach (var filePath in FilePathCollection)
                {
                }
                IsSelectAllChecked = true;
            }  
        }

        private void SelectAllCommandExecute()
        {
            if (IsSelectAllChecked)
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
                return TagType.ID2V22;
            }
            else
            {
                return TagType.none;
            }

        }

        #endregion//Methods
    }
}
