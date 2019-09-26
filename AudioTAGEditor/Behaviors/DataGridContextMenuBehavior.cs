using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Ribbon;
using System.Windows.Interactivity;

namespace AudioTAGEditor.Behaviors
{
    class DataGridContextMenuBehavior :
        Behavior<ContextMenu>
    {
        public DataGrid DataGrid
        {
            get { return (DataGrid)GetValue(DataGridProperty); }
            set { SetValue(DataGridProperty, value); }
        }

        public static readonly DependencyProperty DataGridProperty =
            DependencyProperty.Register(
                "DataGrid",
                typeof(DataGrid),
                typeof(DataGridContextMenuBehavior), 
                new PropertyMetadata(null));

        public DataGrid DG { get; set; }

        protected override void OnAttached()
        {
            base.OnAttached();

            var behavior = AssociatedObject;
            behavior.Loaded += Behavior_Loaded;
            if (DG != null)
                DG.Loaded += DG_Loaded;
        }

        private void DG_Loaded(object sender, RoutedEventArgs e)
        {
            throw new System.NotImplementedException();
        }

        private void Behavior_Loaded(object sender, RoutedEventArgs e)
        {
            if (DataGrid != null)
                DataGrid.ContextMenuOpening += DataGrid_ContextMenuOpening;
        }

        private void DataGrid_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            var s = e.Source;
        }
    }
}
