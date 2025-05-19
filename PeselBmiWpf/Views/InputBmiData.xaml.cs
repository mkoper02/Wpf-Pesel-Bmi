using PeselBmiWpf.Models;
using System.Windows;
using System.Windows.Controls;

namespace PeselBmiWpf.Views
{
    public partial class InputBmiData : UserControl
    {
        public event Action<BmiRecord>? BmiDataAdded;

        public InputBmiData()
        {
            InitializeComponent();
        }

        private bool ValidateInput()
        {
            // Validate input fields
            if (string.IsNullOrWhiteSpace(WeightInputTextBox.Input.Text) ||
                string.IsNullOrWhiteSpace(HeightInputTextBox.Input.Text))
            {
                MessageBox.Show("Wszystkie pola są wymagane.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (!double.TryParse(WeightInputTextBox.Input.Text, out double weight) || weight <= 0)
            {
                MessageBox.Show("Masa musi być liczbą dodatnią.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (!double.TryParse(HeightInputTextBox.Input.Text, out double height) || height <= 0)
            {
                MessageBox.Show("Wzrost musi być liczbą dodatnią.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            return true;
        }

        private void AddBmiButton_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateInput())
            {
                return;
            }

            var bmiRecord = new BmiRecord
            {
                Weight = double.Parse(WeightInputTextBox.Input.Text),
                Height = double.Parse(HeightInputTextBox.Input.Text)
            };

            // Raise the BmiDataAdded event
            BmiDataAdded?.Invoke(bmiRecord);
            ClearBmiInputTextBox();
        }

        private void UpdateBmiButton_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateInput())
            {
                return;
            }

            // Update the binding source for height and weight
            var heightBounding = HeightInputTextBox.Input.GetBindingExpression(TextBox.TextProperty);
            var weightBinding = WeightInputTextBox.Input.GetBindingExpression(TextBox.TextProperty);

            heightBounding?.UpdateSource();
            weightBinding?.UpdateSource();

            MessageBox.Show("Dane pomiaru zostały zaktualizowane.", "Sukces", MessageBoxButton.OK, MessageBoxImage.Information);
            ClearBmiInputTextBox();
        }

        private void ClearBmiButton_Click(object sender, RoutedEventArgs e)
        {
            ClearBmiInputTextBox();
        }

        private void ClearBmiInputTextBox()
        {
            HeightInputTextBox.Input.Clear();
            WeightInputTextBox.Input.Clear();
        }
    }
}