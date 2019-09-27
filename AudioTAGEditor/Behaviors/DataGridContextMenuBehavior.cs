using AudioTAGEditor.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;
using System.Linq;

namespace AudioTAGEditor.Behaviors
{
    class DataGridContextMenuBehavior :
        Behavior<DataGrid>
    {
        private MenuItem activateTagMenuItem;
        private MenuItem deactivateTagMenuItem;
        protected override void OnAttached()
        {
            base.OnAttached();

            var dataGrid = AssociatedObject;
            if (dataGrid != null)
            {
                dataGrid.ContextMenuOpening += DataGrid_ContextMenuOpening;
                
            }  
        }

        private void DataGrid_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            var dataGrid = sender as DataGrid;
            var clickedRow = e.OriginalSource as FrameworkElement;
            if (clickedRow != null && dataGrid != null)
            {
                var contextMenuItems = dataGrid.ContextMenu?.Items;
                var contextMenuItemsEnumerable = contextMenuItems?.Cast<MenuItem>();

                if (activateTagMenuItem == null)
                {
                    activateTagMenuItem =
                        contextMenuItemsEnumerable?
                        .FirstOrDefault(c => c.Header.ToString() == "Activate Tag");

                    activateTagMenuItem.Click += ActivateTagMenuItem_Click;
                }
                    
                if (deactivateTagMenuItem == null)
                {
                    deactivateTagMenuItem =
                        contextMenuItemsEnumerable?
                        .FirstOrDefault(c => c.Header.ToString() == "Deactivate Tag");
                    deactivateTagMenuItem.Click += DeactivateTagMenuItem_Click;
                }
                    
                var audioFileViewModel = clickedRow.DataContext as AudioFileViewModel;
                if (audioFileViewModel != null)
                {
                    if (audioFileViewModel.HasTag)
                    {
                        activateTagMenuItem.IsEnabled = false;
                        deactivateTagMenuItem.IsEnabled = true;
                    }
                    else
                    {
                        activateTagMenuItem.IsEnabled = true;
                        deactivateTagMenuItem.IsEnabled = false;
                    }
                }
            }
        }

        private void ActivateTagMenuItem_Click(object sender, RoutedEventArgs e)
        {
            throw new System.NotImplementedException();
        }

        private void DeactivateTagMenuItem_Click(object sender, RoutedEventArgs e)
        {
            throw new System.NotImplementedException();
        }  
    }
}
