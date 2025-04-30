using PeselBmiWpf.Models;
using System.Windows;
using System.Windows.Controls;

namespace PeselBmiWpf.Views.UserControls
{
    public partial class InputPerson : UserControl
    {
        public event Action<Person> PersonAdded;

        public InputPerson()
        {
            InitializeComponent();
        }

        private void AddPersonButton_Click(object sender, RoutedEventArgs e)
        {
            // Validate input fields
            if (string.IsNullOrWhiteSpace(FirstNameInputTextBox.Input.Text) ||
                string.IsNullOrWhiteSpace(LastNameInputTextBox.Input.Text) ||
                string.IsNullOrWhiteSpace(PeselInputTextBox.Input.Text))
            {
                MessageBox.Show("Wszystkie pola są wymagane.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!Person.IsPeselValid(PeselInputTextBox.Input.Text))
            {
                MessageBox.Show("Pesel jest niepoprawny.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
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

            FirstNameInputTextBox.Input.Text = string.Empty;
            LastNameInputTextBox.Input.Text = string.Empty;
            PeselInputTextBox.Input.Text = string.Empty;
        }

        private void UpdatePersonButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
