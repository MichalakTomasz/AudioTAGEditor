using AudioTAGEditor.Models;
using AudioTAGEditor.Services;
using AudioTAGEditor.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace AudioTAGEditor.Behaviors
{
    public class DataGridEditBehavior : Behavior<DataGrid>
    {
        #region Dependency Properties

        public IHistoryService HistoryService
        {
            get { return (IHistoryService)GetValue(HistoryServiceProperty); }
            set { SetValue(HistoryServiceProperty, value); }
        }

        public static readonly DependencyProperty HistoryServiceProperty =
            DependencyProperty.Register(
                "HistoryService",
                typeof(IHistoryService),
                typeof(DataGridEditBehavior),
                new PropertyMetadata(null));

        public IAudiofileConverter AudioFileConverter
        {
            get { return (IAudiofileConverter)GetValue(AudioFileConverterProperty); }
            set { SetValue(AudioFileConverterProperty, value); }
        }

        public static readonly DependencyProperty AudioFileConverterProperty =
            DependencyProperty.Register(
                "AudioFileConverter",
                typeof(IAudiofileConverter),
                typeof(DataGridEditBehavior),
                new PropertyMetadata(null));

        public IFileService FileService
        {
            get { return (IFileService)GetValue(FileServiceProperty); }
            set { SetValue(FileServiceProperty, value); }
        }

        public static readonly DependencyProperty FileServiceProperty =
            DependencyProperty.Register(
                "FileService",
                typeof(IFileService),
                typeof(DataGridEditBehavior),
                new PropertyMetadata(null));

        public IID3Service ID3v1Service
        {
            get { return (IID3Service)GetValue(ID3v1ServiceProperty); }
            set { SetValue(ID3v1ServiceProperty, value); }
        }

        public static readonly DependencyProperty ID3v1ServiceProperty =
            DependencyProperty.Register(
                "ID3v1Service",
                typeof(IID3Service),
                typeof(DataGridEditBehavior),
                new PropertyMetadata(null));

        public IID3Service ID3v2Service
        {
            get { return (IID3Service)GetValue(ID3v2ServiceProperty); }
            set { SetValue(ID3v2ServiceProperty, value); }
        }

        public static readonly DependencyProperty ID3v2ServiceProperty =
            DependencyProperty.Register(
                "ID3v2Service",
                typeof(IID3Service),
                typeof(DataGridEditBehavior),
                new PropertyMetadata(null));

        public IAudiofileComparerService AudioFileComparerService
        {
            get { return (IAudiofileComparerService)GetValue(AudioFileComparerServiceProperty); }
            set { SetValue(AudioFileComparerServiceProperty, value); }
        }

        public static readonly DependencyProperty AudioFileComparerServiceProperty =
            DependencyProperty.Register(
                "AudioFileComparerService",
                typeof(IAudiofileComparerService),
                typeof(DataGridEditBehavior),
                new PropertyMetadata(null));

        public string SelectedPath
        {
            get { return (string)GetValue(SelectedPathProperty); }
            set { SetValue(SelectedPathProperty, value); }
        }

        public static readonly DependencyProperty SelectedPathProperty =
            DependencyProperty.Register(
                "SelectedPath",
                typeof(string),
                typeof(DataGridEditBehavior),
                new PropertyMetadata(null));

        public ExplorerTreeView.ExplorerTreeView ExplorerTreeView
        {
            get { return (ExplorerTreeView.ExplorerTreeView)GetValue(ExplorerTreeViewProperty); }
            set { SetValue(ExplorerTreeViewProperty, value); }
        }

        public static readonly DependencyProperty ExplorerTreeViewProperty =
            DependencyProperty.Register(
                "ExplorerTreeView",
                typeof(ExplorerTreeView.ExplorerTreeView),
                typeof(DataGridEditBehavior),
                new PropertyMetadata(null));
        
        public bool IsCheckedID3v1
        {
            get { return (bool)GetValue(IsCheckedID3v1Property); }
            set { SetValue(IsCheckedID3v1Property, value); }
        }

        public static readonly DependencyProperty IsCheckedID3v1Property =
            DependencyProperty.Register(
                "IsCheckedID3v1",
                typeof(bool),
                typeof(DataGridEditBehavior),
                new PropertyMetadata(null));

        public bool IsCheckedID3v2
        {
            get { return (bool)GetValue(IsCheckedID3v2Property); }
            set { SetValue(IsCheckedID3v2Property, value); }
        }

        public static readonly DependencyProperty IsCheckedID3v2Property =
            DependencyProperty.Register(
                "IsCheckedID3v2",
                typeof(bool),
                typeof(DataGridEditBehavior),
                new PropertyMetadata(null));


        public int HistoryCount
        {
            get { return (int)GetValue(HistoryCountProperty); }
            set { SetValue(HistoryCountProperty, value); }
        }

        public static readonly DependencyProperty HistoryCountProperty =
            DependencyProperty.Register(
                "HistoryCount", 
                typeof(int), 
                typeof(DataGridEditBehavior), 
                new PropertyMetadata(0));

        public int HistoryPosition
        {
            get { return (int)GetValue(HistoryPositionProperty); }
            set { SetValue(HistoryPositionProperty, value); }
        }

        public static readonly DependencyProperty HistoryPositionProperty =
            DependencyProperty.Register(
                "HistoryPosition", 
                typeof(int), 
                typeof(DataGridEditBehavior), 
                new PropertyMetadata(0));


        public ILogService LogService
        {
            get { return (ILogService)GetValue(LogServiceProperty); }
            set { SetValue(LogServiceProperty, value); }
        }

        public static readonly DependencyProperty LogServiceProperty =
            DependencyProperty.Register(
                "LogService", 
                typeof(ILogService), 
                typeof(DataGridEditBehavior), new PropertyMetadata(null));


        public string LogMessage
        {
            get { return (string)GetValue(LogMessageProperty); }
            set { SetValue(LogMessageProperty, value); }
        }

        public static readonly DependencyProperty LogMessageProperty =
            DependencyProperty.Register(
                "LogMessage", 
                typeof(string), 
                typeof(DataGridEditBehavior), 
                new PropertyMetadata(null));


        public LogMessageStatusType LogMessageStatusType
        {
            get { return (LogMessageStatusType)GetValue(LogMessageStatusTypeProperty); }
            set { SetValue(LogMessageStatusTypeProperty, value); }
        }

        public static readonly DependencyProperty LogMessageStatusTypeProperty =
            DependencyProperty.Register(
                "LogMessageStatusType", 
                typeof(LogMessageStatusType), 
                typeof(DataGridEditBehavior), 
                new PropertyMetadata(LogMessageStatusType.None));

        #endregion // Dependency Properties

        #region Methods

        protected override void OnAttached()
        {
            base.OnAttached();

            var dataGrid = AssociatedObject;
            if (dataGrid != null)
            {
                dataGrid.BeginningEdit += DataGrid_BeginningEdit;
                dataGrid.CellEditEnding += DataGrid_CellEditEnding;
            }
        }

        private void DataGrid_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            if (e.Column.Header is CheckBox)
                return;

            var audioFileViewModel = e.Row.DataContext as AudiofileViewModel;
            var audioFile = AudioFileConverter.AudiofileViewModelToAudiofile(audioFileViewModel);

            audioFileBeforeEdit =  audioFile;
        }

        private void DataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (e.EditAction == DataGridEditAction.Commit && !(e.Column.Header is CheckBox))
            {
                var audiofileViewModel = e.EditingElement.DataContext as AudiofileViewModel;
                var filename = $"{SelectedPath}{audiofileViewModel.Filename}";
                

                if (!audiofileViewModel.HasErrors)
                {
                    var newAudioFile = AudioFileConverter.AudiofileViewModelToAudiofile(audiofileViewModel);
                    var audioFileFullPath = $"{SelectedPath}{newAudioFile.Filename}";
                    LogItem log;

                    switch (e.Column.Header)
                    {
                        case "File Name":
                            var oldFilename = audioFileBeforeEdit.Filename;
                            var exist = FileService.Exist(filename);
                            if (exist)
                            {
                                log = LogService.Add(LogMessageStatusType.Error, "This folder contains file with the same name.");
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

                            log = LogService.Add(LogMessageStatusType.Information, "Filename changed.");
                            LogMessageStatusType = log.LogMessageStatusType;
                            LogMessage = log.Message;
                            break;
                        default:
                            if (AudioFileComparerService.AreTheSame(audioFileBeforeEdit, newAudioFile))
                                return;

                            if (IsCheckedID3v1)
                            {
                                ID3v1Service.UpdateTag(newAudioFile, audioFileFullPath, TagVersion.ID3V11);
                                HistoryService.Add(audioFileBeforeEdit, ChangeActionType.ID3v1, SelectedPath);
                                UpdateHistoryProperties();

                                log = LogService.Add(LogMessageStatusType.Information, "ID3v1 Tag changed.");
                                LogMessageStatusType = log.LogMessageStatusType;
                                LogMessage = log.Message;
                            }
                                
                            if (IsCheckedID3v2)
                            {
                                ID3v2Service.UpdateTag(newAudioFile, audioFileFullPath, TagVersion.ID3V20);
                                HistoryService.Add(audioFileBeforeEdit, ChangeActionType.ID3v2, SelectedPath);
                                UpdateHistoryProperties();

                                log = LogService.Add(LogMessageStatusType.Information, "ID3v2 Tag changed.");
                                LogMessageStatusType = log.LogMessageStatusType;
                                LogMessage = log.Message;
                            }
                            break;
                    }
                }
            }
        }

        private void UpdateHistoryProperties()
        {
            HistoryCount = HistoryService.Count;
            HistoryPosition = HistoryService.Position;
        }
        #endregion// Methods

        #region Private Fields

        private Audiofile audioFileBeforeEdit;

        #endregion// Private Fields
    }
}
