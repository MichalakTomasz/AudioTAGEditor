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

namespace AudioTAGEditor.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        #region Properties

        IAudioTAGService audioTAGService;

        #endregion//Properties

        #region Constructor

        public MainWindowViewModel(IAudioTAGService audioTAGService)
        {
            this.audioTAGService = audioTAGService;
            FilesFilter = ".mp3|.flac|.mpc|.ogg|.aac";
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

        private bool selectAll;
        public bool SelectAll
        {
            get { return selectAll; }
            set { SetProperty(ref selectAll, value); }
        }

        private TAGType tagType;
        public TAGType TAGType
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

        #endregion//Commands

        #region Methods

        private void ExpandCommandExecute()
        {
            IsEnabledDataGrid = (filePathCollection?.Count() > 0);
            if (IsEnabledDataGrid)
            {
                foreach (var filePath in FilePathCollection)
                {
                    var tempAudioFile = audioTAGService.GetTagData(filePath, TAGType);
                    AudioFiles.Add(tempAudioFile);
                }
            }  
        }

        private void SelectAllCommandExecute()
        {
           
        }

        #endregion//Methods
    }
}
