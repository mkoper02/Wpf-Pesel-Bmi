using System.Windows;
using System.Windows.Controls;

namespace PeselBmiWpf.View
{
    public partial class InputTextBox : UserControl
    {
        public InputTextBox()
        {
            InitializeComponent();
        }

        // Dependency Property for Text
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register(
                "Text",
                typeof(string),
                typeof(InputTextBox),
                new PropertyMetadata(string.Empty));

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }
    }
}
