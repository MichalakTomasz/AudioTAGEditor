using AudioTAGEditor.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;
using System.Linq;
using AudioTAGEditor.Services;
using System;
using AudioTAGEditor.Models;

namespace AudioTAGEditor.Behaviors
{
    class DataGridContextMenuDeactivateTagBehavior :
        Behavior<DataGrid>
    {
        #region Dependency Properties
        
        public string SelectedPath
        {
            get { return (string)GetValue(SelectedPathProperty); }
            set { SetValue(SelectedPathProperty, value); }
        }

        public static readonly DependencyProperty SelectedPathProperty =
            DependencyProperty.Register(
                "SelectedPath", 
                typeof(string), 
                typeof(DataGridContextMenuDeactivateTagBehavior), 
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
                typeof(DataGridContextMenuDeactivateTagBehavior), 
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
                typeof(DataGridContextMenuDeactivateTagBehavior), 
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
                typeof(DataGridContextMenuDeactivateTagBehavior), 
                new PropertyMetadata(null));

        #endregion // Dependency Properties

        #region Methods

        protected override void OnAttached()
        {
            base.OnAttached();

            var dataGrid = AssociatedObject;
            if (dataGrid != null)
                dataGrid.ContextMenuOpening += DataGrid_ContextMenuOpening;
        }

        private void DataGrid_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            var dataGrid = sender as DataGrid;
            var clickedRow = e.OriginalSource as FrameworkElement;
            if (clickedRow != null && dataGrid != null)
            {
                var contextMenuItems = dataGrid.ContextMenu?.Items;
                var contextMenuItemsEnumerable = contextMenuItems?.Cast<MenuItem>();
                
                if (menuItemDeactivateTag == null)
                {
                    menuItemDeactivateTag =
                        contextMenuItemsEnumerable?
                        .FirstOrDefault(c => c.Header.ToString() == "Deactivate Tag");

                    if (menuItemDeactivateTag != null)
                        menuItemDeactivateTag.Click += MenuItemDeactivateTag_Click;
                }

                audioFileViewModel = clickedRow.DataContext as AudioFileViewModel;
                if (audioFileViewModel != null)
                    menuItemDeactivateTag.IsEnabled = audioFileViewModel.HasTag;
            }
        }

        private void MenuItemDeactivateTag_Click(object sender, RoutedEventArgs e)
        {
            if (audioFileViewModel != null)
            {
                var fileFullPath = $"{SelectedPath}{audioFileViewModel.Filename}";
                switch (audioFileViewModel.TagType)
                {
                    case TagType.ID3V1:
                        ID3v2Service.RemoveTag(fileFullPath);
                        break;
                    case TagType.ID3V2:
                        ID3v2Service.RemoveTag(fileFullPath);
                        break;
                }
                ExplorerTreeView.ExecuteSelectedNodeEvents();
            }
            else new Exception("Not set view model reference");
        }

        #endregion // Methods

        #region Field

        private MenuItem menuItemDeactivateTag;
        private AudioFileViewModel audioFileViewModel;

        #endregion // Fielda
    }
}
