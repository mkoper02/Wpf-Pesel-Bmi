using PeselBmiWpf.Models;
using System.Windows;
using System.Windows.Controls;

namespace PeselBmiWpf.Views.UserControls
{
    public partial class InputBmiData : UserControl
    {
        public event Action<BmiRecord> BmiDataAdded;

        public InputBmiData()
        {
            InitializeComponent();
        }

        private void AddBmiButton_Click(object sender, RoutedEventArgs e)
        {
            // Validate input fields
            if (string.IsNullOrWhiteSpace(WeightInputTextBox.Input.Text) ||
                string.IsNullOrWhiteSpace(HeightInputTextBox.Input.Text))
            {
                MessageBox.Show("Wszystkie pola są wymagane.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!double.TryParse(WeightInputTextBox.Input.Text, out double weight) || weight <= 0)
            {
                MessageBox.Show("Masa musi być liczbą dodatnią.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!double.TryParse(HeightInputTextBox.Input.Text, out double height) || height <= 0)
            {
                MessageBox.Show("Wzrost musi być liczbą dodatnią.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var bmiRecord = new BmiRecord
            {
                Weight = weight,
                Height = height
            };

            // Raise the BmiDataAdded event
            BmiDataAdded?.Invoke(bmiRecord);

            WeightInputTextBox.Input.Text = string.Empty;
            HeightInputTextBox.Input.Text = string.Empty;
        }

        private void UpdateBmiButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
