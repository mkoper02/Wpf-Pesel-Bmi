using PeselBmiWpf.Models;
using System.Windows;
using System.Windows.Controls;

namespace PeselBmiWpf.Views.UserControls
{
    public partial class InputPerson : UserControl
    {
        public event Action<Person>? PersonAdded;

        public InputPerson()
        {
            InitializeComponent();
        }

        private bool ValidateInput()
        {
            // Validate input fields
            if (string.IsNullOrWhiteSpace(FirstNameInputTextBox.Input.Text) ||
                string.IsNullOrWhiteSpace(LastNameInputTextBox.Input.Text) ||
                string.IsNullOrWhiteSpace(PeselInputTextBox.Input.Text))
            {
                MessageBox.Show("Wszystkie pola są wymagane.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (!Person.IsPeselValid(PeselInputTextBox.Input.Text))
            {
                MessageBox.Show("Pesel jest niepoprawny.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            return true;
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
                Pesel = PeselInputTextBox.Input.Text
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

            // Update the binding source for pesel
            var firstNameBinding = FirstNameInputTextBox.Input.GetBindingExpression(TextBox.TextProperty);
            var lastNameBinding = LastNameInputTextBox.Input.GetBindingExpression(TextBox.TextProperty);
            var peselBinding = PeselInputTextBox.Input.GetBindingExpression(TextBox.TextProperty);

            firstNameBinding?.UpdateSource();
            lastNameBinding?.UpdateSource();
            peselBinding?.UpdateSource();

            MessageBox.Show("Dane zostały zaktualizowane.", "Sukces", MessageBoxButton.OK, MessageBoxImage.Information);
            ClearPersonInputTextBox();
        }

        private void ClearPersonButton_Click(object sender, RoutedEventArgs e)
        {
            ClearPersonInputTextBox();
        }

        private void ClearPersonInputTextBox()
        {
            FirstNameInputTextBox.Input.Text = string.Empty;
            LastNameInputTextBox.Input.Text = string.Empty;
            PeselInputTextBox.Input.Text = string.Empty;
        }
    }
}

