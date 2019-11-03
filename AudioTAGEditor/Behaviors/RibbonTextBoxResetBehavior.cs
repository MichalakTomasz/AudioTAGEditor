using System.Windows.Controls;
using System.Windows.Controls.Ribbon;
using System.Windows.Interactivity;

namespace AudioTAGEditor.Behaviors
{
    public class RibbonTextBoxResetBehavior : Behavior<RibbonTextBox>
    {
        protected override void OnAttached()
        {
            base.OnAttached();

            var textBox = AssociatedObject as TextBox;
            if (textBox != null)
                textBox.TextChanged += TextBox_TextChanged;
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = AssociatedObject as TextBox;
            if (textBox?.Text == string.Empty)
                textBox.Text = null;
        }
    }
}
