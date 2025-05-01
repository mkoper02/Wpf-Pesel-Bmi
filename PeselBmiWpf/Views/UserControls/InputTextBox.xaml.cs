using System.Windows;
using System.Windows.Controls;

namespace PeselBmiWpf.Views.UserControls
{
    public partial class InputTextBox : UserControl
    {
        public InputTextBox()
        {
            InitializeComponent();
        }

        // Dependency Property for LabelText
        public static readonly DependencyProperty LabelTextProperty =
            DependencyProperty.Register(
                "LabelText",
                typeof(string),
                typeof(InputTextBox),
                new PropertyMetadata(string.Empty));

        public string LabelText
        {
            get => (string)GetValue(LabelTextProperty);
            set => SetValue(LabelTextProperty, value);
        }

        // Dependency Property for Text
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register(
                "Text",
                typeof(string),
                typeof(InputTextBox),
                new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }
    }
}
