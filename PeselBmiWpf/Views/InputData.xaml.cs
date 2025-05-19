using PeselBmiWpf.Models;
using System.Windows;
using System.Windows.Controls;

namespace PeselBmiWpf.Views
{
    public partial class InputData : UserControl
    {
        public event Action<Person>? PersonAdded;

        public InputData()
        {
            InitializeComponent();
        }

        private void AddPersonButton_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateInput())
            {
                return;
            }

            var person = new Person
            {
                FirstName = FirstNameInputTextBox.Input.Text,
                LastName = LastNameInputTextBox.Input.Text,
                Pesel = PeselInputTextBox.Input.Text,
                Height = double.Parse(HeightInputTextBox.Input.Text),
                Weight = double.Parse(WeightInputTextBox.Input.Text)
            };

            // Raise the PersonAdded event
            PersonAdded?.Invoke(person);
            ClearPersonInputTextBox();
        }

        private void UpdatePersonButton_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateInput())
            {
                return;
            }

            // Update the binding source
            var firstNameBinding = FirstNameInputTextBox.Input.GetBindingExpression(TextBox.TextProperty);
            var lastNameBinding = LastNameInputTextBox.Input.GetBindingExpression(TextBox.TextProperty);
            var peselBinding = PeselInputTextBox.Input.GetBindingExpression(TextBox.TextProperty);
            var heightBinding = HeightInputTextBox.Input.GetBindingExpression(TextBox.TextProperty);
            var weightBinding = WeightInputTextBox.Input.GetBindingExpression(TextBox.TextProperty);

            firstNameBinding?.UpdateSource();
            lastNameBinding?.UpdateSource();
            peselBinding?.UpdateSource();
            heightBinding?.UpdateSource();
            weightBinding?.UpdateSource();

            MessageBox.Show("Dane zostały zaktualizowane.", "Sukces", MessageBoxButton.OK, MessageBoxImage.Information);
            ClearPersonInputTextBox();
        }

        private void ClearPersonButton_Click(object sender, RoutedEventArgs e)
        {
            ClearPersonInputTextBox();
        }

        private bool ValidateInput()
        {
            // Validate input fields
            if (string.IsNullOrWhiteSpace(FirstNameInputTextBox.Input.Text) ||
                string.IsNullOrWhiteSpace(LastNameInputTextBox.Input.Text) ||
                string.IsNullOrWhiteSpace(PeselInputTextBox.Input.Text) ||
                string.IsNullOrEmpty(HeightInputTextBox.Input.Text) ||
                string.IsNullOrEmpty(WeightInputTextBox.Input.Text))
            {
                MessageBox.Show("Wszystkie pola są wymagane.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (!Person.IsPeselValid(PeselInputTextBox.Input.Text))
            {
                MessageBox.Show("Pesel jest niepoprawny.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (!double.TryParse(HeightInputTextBox.Input.Text, out double height) || height <= 0)
            {
                MessageBox.Show("Wzrost musi być liczbą dodatnią.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (!double.TryParse(WeightInputTextBox.Input.Text, out double weight) || weight <= 0)
            {
                MessageBox.Show("Waga musi być liczbą dodatnią.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            return true;
        }

        private void ClearPersonInputTextBox()
        {
            FirstNameInputTextBox.Input.Clear();
            LastNameInputTextBox.Input.Clear();
            PeselInputTextBox.Input.Clear();
            HeightInputTextBox.Input.Clear();
            WeightInputTextBox.Input.Clear();
        }
    }
}
