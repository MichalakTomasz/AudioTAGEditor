using AudioTAGEditor.Models;
using AudioTAGEditor.Services;
using AudioTAGEditor.ViewModels;
using System;
using System.Linq;
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

        public IAudioFileConverter AudioFileConverter
        {
            get { return (IAudioFileConverter)GetValue(AudioFileConverterProperty); }
            set { SetValue(AudioFileConverterProperty, value); }
        }

        public static readonly DependencyProperty AudioFileConverterProperty =
            DependencyProperty.Register(
                "AudioFileConverter",
                typeof(IAudioFileConverter),
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

        public IAudioFileComparerService AudioFileComparerService
        {
            get { return (IAudioFileComparerService)GetValue(AudioFileComparerServiceProperty); }
            set { SetValue(AudioFileComparerServiceProperty, value); }
        }

        public static readonly DependencyProperty AudioFileComparerServiceProperty =
            DependencyProperty.Register(
                "AudioFileComparerService",
                typeof(IAudioFileComparerService),
                typeof(DataGridEditBehavior),
                new PropertyMetadata(null));

        #endregion// Dependency Properties

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

            var audioFileViewModel = e.Row.DataContext as AudioFileViewModel;
            var audioFile = AudioFileConverter.AdioFileViewModelToAudioFile(audioFileViewModel);

            switch (e.Column.Header)
            {
                case "File Name":
                    tempID = HistoryService.PushOldValue(audioFile, ChangeActionType.Filename, SelectedPath);
                    break;
                default:
                    var editActionType = ChangeActionType.None;
                    switch (audioFile.TagType)
                    {
                        case TagType.ID3V1:
                            editActionType = ChangeActionType.ID3v1;
                            break;
                        case TagType.ID3V2:
                            editActionType = ChangeActionType.ID3v2;
                            break;
                    }
                    tempID = HistoryService.PushOldValue(audioFile, editActionType, SelectedPath);
                    break;
            }
        }

        private void DataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (e.EditAction == DataGridEditAction.Commit)
            {
                var audioFileViewModel = e.EditingElement.DataContext as AudioFileViewModel;
                if (!audioFileViewModel.HasErrors)
                {
                    var audioFile = AudioFileConverter.AdioFileViewModelToAudioFile(audioFileViewModel);
                    var audioFileFullPath = $"{SelectedPath}{audioFileViewModel.Filename}";
                    var historyObject = HistoryService.Peek();
                    if (tempID == historyObject.ID)
                    {
                        switch (e.Column.Header)
                        {
                            case "File Name":
                                var oldFilename = historyObject.AudioFileChanges?.LastOrDefault()?.Old?.Filename;
                                if (audioFile.Filename == oldFilename)
                                {
                                    HistoryService.Pop();
                                    break;
                                }
                                var oldAaudioFile = historyObject.AudioFileChanges.LastOrDefault().Old;
                                var oldFilePath = $"{historyObject.Path}{oldAaudioFile.Filename}";
                                FileService.Rename(oldFilePath, audioFileViewModel.Filename);
                                HistoryService.PushChange(tempID, audioFile);
                                ExplorerTreeView.Refresh();
                                break;
                            default:
                                var oldAudioFile = historyObject.AudioFileChanges?.LastOrDefault()?.Old;
                                if (AudioFileComparerService.AreTheSame(oldAudioFile, audioFile))
                                {
                                    HistoryService.Pop();
                                    return;
                                }
                                switch (audioFileViewModel.TagType)
                                {
                                    case TagType.ID3V1:
                                        ID3v1Service.UpdateTag(audioFile, audioFileFullPath, TagVersion.ID3V11);
                                        break;
                                    case TagType.ID3V2:
                                        ID3v2Service.UpdateTag(audioFile, audioFileFullPath, TagVersion.ID3V20);
                                        break;
                                }
                                break;
                        }
                    }
                    else throw new Exception("Wrong history object");
                }
            }
        }

        #endregion// Methods

        #region Private Fields

        private Guid tempID;

        #endregion// Private Fields
    }
}
