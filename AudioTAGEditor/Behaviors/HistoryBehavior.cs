using AudioTAGEditor.Models;
using AudioTAGEditor.ViewModels;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls.Ribbon;
using System.Windows.Interactivity;
using System.Linq;
using AudioTAGEditor.Services;
using Prism.Events;
using System;

namespace AudioTAGEditor.Behaviors
{
    public class HistoryBehavior : Behavior<Ribbon>
    {
        #region Dependency Properties

        public IEnumerable<AudiofileViewModel> Audiofiles
        {
            get { return (IEnumerable<AudiofileViewModel>)GetValue(AudiofilesProperty); }
            set { SetValue(AudiofilesProperty, value); }
        }

        public static readonly DependencyProperty AudiofilesProperty =
            DependencyProperty.Register(
                "Audiofiles",
                typeof(IEnumerable<AudiofileViewModel>),
                typeof(HistoryBehavior),
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
                typeof(HistoryBehavior),
                new PropertyMetadata(false));

        public bool IsCheckedID3v2
        {
            get { return (bool)GetValue(IsCheckedID3v2Property); }
            set { SetValue(IsCheckedID3v2Property, value); }
        }

        public static readonly DependencyProperty IsCheckedID3v2Property =
            DependencyProperty.Register(
                "IsCheckedID3v2",
                typeof(bool),
                typeof(HistoryBehavior),
                new PropertyMetadata(false));

        public IAudiofileConverter AudiofileConverter
        {
            get { return (IAudiofileConverter)GetValue(AudiofileConverterProperty); }
            set { SetValue(AudiofileConverterProperty, value); }
        }

        public static readonly DependencyProperty AudiofileConverterProperty =
            DependencyProperty.Register(
                "AudiofileConverter",
                typeof(IAudiofileConverter),
                typeof(HistoryBehavior),
                new PropertyMetadata(null));

        public IHistoryService HistoryService
        {
            get { return (IHistoryService)GetValue(HistoryServiceProperty); }
            set { SetValue(HistoryServiceProperty, value); }
        }

        public static readonly DependencyProperty HistoryServiceProperty =
            DependencyProperty.Register(
                "HistoryService",
                typeof(IHistoryService),
                typeof(HistoryBehavior),
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
                typeof(HistoryBehavior),
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
                typeof(HistoryBehavior),
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
                typeof(HistoryBehavior),
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
                typeof(HistoryBehavior),
                new PropertyMetadata(0));

        public IID3Service ID3v1Service
        {
            get { return (IID3Service)GetValue(ID3v1ServiceProperty); }
            set { SetValue(ID3v1ServiceProperty, value); }
        }

        public static readonly DependencyProperty ID3v1ServiceProperty =
            DependencyProperty.Register(
                "ID3v1Service",
                typeof(IID3Service),
                typeof(HistoryBehavior),
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
                typeof(HistoryBehavior),
                new PropertyMetadata(null));

        public IEventAggregator EventAggregator
        {
            get { return (IEventAggregator)GetValue(EventAggregatorProperty); }
            set { SetValue(EventAggregatorProperty, value); }
        }

        public static readonly DependencyProperty EventAggregatorProperty =
            DependencyProperty.Register(
                "EventAggregator",
                typeof(IEventAggregator),
                typeof(HistoryBehavior),
                new PropertyMetadata(null));


        public ILogService LogService
        {
            get { return (ILogService)GetValue(LogServiceProperty); }
            set { SetValue(LogServiceProperty, value); }
        }

        public static readonly DependencyProperty LogServiceProperty =
            DependencyProperty.Register(
                "LogService", 
                typeof(ILogService), 
                typeof(HistoryBehavior), 
                new PropertyMetadata(null));


        public string LogMessage
        {
            get { return (string)GetValue(LogMessageProperty); }
            set { SetValue(LogMessageProperty, value); }
        }

        public static readonly DependencyProperty LogMessageProperty =
            DependencyProperty.Register(
                "LogMessage", 
                typeof(string), 
                typeof(HistoryBehavior), 
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
                typeof(HistoryBehavior), 
                new PropertyMetadata(LogMessageStatusType.None));

        public ExplorerTreeView.ExplorerTreeView ExplorerTreeView
        {
            get { return (ExplorerTreeView.ExplorerTreeView)GetValue(ExplorerTreeViewProperty); }
            set { SetValue(ExplorerTreeViewProperty, value); }
        }

        public static readonly DependencyProperty ExplorerTreeViewProperty =
            DependencyProperty.Register(
                "ExplorerTreeView",
                typeof(ExplorerTreeView.ExplorerTreeView),
                typeof(HistoryBehavior),
                new PropertyMetadata(null));

        #endregion // Dependency Properties

        #region Methods

        protected override void OnAttached()
        {
            base.OnAttached();
            var ribbon = AssociatedObject;
            var tabs = ribbon.Items;
            var historyTab = tabs[2] as RibbonTab;
            var ribbonGroup = historyTab.Items[0] as RibbonGroup;
            var ribbonGroupItems = ribbonGroup.Items;
            ribbonButtonUndo = ribbonGroupItems[0] as RibbonButton;
            ribbonButtonRedo = ribbonGroupItems[1] as RibbonButton;
            ribbonButtonConfirm = ribbonGroupItems[2] as RibbonButton;
            ribbonButtonCancel = ribbonGroupItems[3] as RibbonButton;

            ribbonButtonUndo.PreviewMouseLeftButtonDown += 
                RibbonButtonUndo_PreviewMouseLeftButtonDown;
            ribbonButtonRedo.PreviewMouseLeftButtonDown += 
                RibbonButtonRedo_PreviewMouseLeftButtonDown;
            ribbonButtonConfirm.PreviewMouseLeftButtonDown += 
                RibbonButtonConfirm_PreviewMouseLeftButtonDown;
            ribbonButtonCancel.PreviewMouseLeftButtonDown += 
                RibbonButtonCancel_PreviewMouseLeftButtonDown;
        }

        private void RibbonButtonCancel_PreviewMouseLeftButtonDown(
            object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            throw new System.NotImplementedException();
        }

        private void RibbonButtonConfirm_PreviewMouseLeftButtonDown(
            object sender, System.Windows.Input.MouseButtonEventArgs e)
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
                            Audiofiles as IEnumerable<AudiofileID3v1ViewModel>)
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
                        var currentFilename = HistoryService.GetCurrentFilename(a.ID);
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
                ExplorerTreeView.Refresh();
                
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

        private void RibbonButtonRedo_PreviewMouseLeftButtonDown(
            object sender, System.Windows.Input.MouseButtonEventArgs e)
            => SetHistoryStepToMainGrid(HistoryStepType.Redo);

        private void RibbonButtonUndo_PreviewMouseLeftButtonDown(
            object sender, System.Windows.Input.MouseButtonEventArgs e)
            => SetHistoryStepToMainGrid(HistoryStepType.Undo);

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

        private void UpdateHistoryProperties()
        {
            HistoryCount = HistoryService.Count;
            HistoryPosition = HistoryService.Position;
        }

        #endregion // Methods

        #region Fields

        RibbonButton ribbonButtonUndo;
        RibbonButton ribbonButtonRedo;
        RibbonButton ribbonButtonConfirm;
        RibbonButton ribbonButtonCancel;

        #endregion // Fields
    }
}
