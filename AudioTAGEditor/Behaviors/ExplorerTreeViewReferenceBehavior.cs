using ExplorerTreeView;
using System.Windows;
using System.Windows.Interactivity;

namespace AudioTAGEditor.Behaviors
{
    class ExplorerTreeViewReferenceBehavior
        : Behavior<ExplorerTreeView.ExplorerTreeView>
    {
        public ExplorerTreeView.ExplorerTreeView ExplorerTreeView
        {
            get { return (ExplorerTreeView.ExplorerTreeView)GetValue(ExplorerTreeViewProperty); }
            set { SetValue(ExplorerTreeViewProperty, value); }
        }

        public static readonly DependencyProperty ExplorerTreeViewProperty =
            DependencyProperty.Register(
                "ExplorerTreeView", 
                typeof(ExplorerTreeView.ExplorerTreeView), 
                typeof(ExplorerTreeViewReferenceBehavior), 
                new PropertyMetadata(null));

        protected override void OnAttached()
        {
            base.OnAttached();
            ExplorerTreeView = AssociatedObject;
        }
    }
}
