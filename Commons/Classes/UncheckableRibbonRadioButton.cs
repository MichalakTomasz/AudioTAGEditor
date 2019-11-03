using System.Windows;
using System.Windows.Controls.Ribbon;

namespace Commons
{
    public class UncheckableRibbonRadioButton : RibbonRadioButton
    {
        public bool CanUncheck
        {
            get { return (bool)GetValue(CanUncheckProperty); }
            set { SetValue(CanUncheckProperty, value); }
        }

        public static readonly DependencyProperty CanUncheckProperty =
            DependencyProperty.Register(
                "CanUncheck", 
                typeof(bool), 
                typeof(UncheckableRibbonRadioButton), 
                new PropertyMetadata(false));

        protected override void OnClick()
        {
            var wasChecked = IsChecked;
            base.OnClick();
            if (CanUncheck &&
                wasChecked != null &&
                wasChecked.Value)
                IsChecked = false;
        }
    }
}
