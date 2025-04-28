using System.Windows;

namespace PeselBmiWpf
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void CalculateBmiButton_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateInputs())
            {
                return;
            }

            BmiResultLabel.Content = string.Empty;

            var pesel = PeselInputTextBox.Input.Text;
            var dateOfBirth = Calculations.GetDateOfBirth(pesel);
            var sex = Calculations.IsMale(pesel) ? "Mężczyzna" : "Kobieta";

            var height = double.Parse(HeightInputTextBox.Input.Text);
            var weight = double.Parse(WeightInputTextBox.Input.Text);
            var bmi = Calculations.CalculateBmi(weight, height);
            var bmiCategory = Calculations.GetBmiCategory(bmi);

            var result = "Imię: " + FirstNameInputTextBox.Input.Text + 
                         ", Nazwisko: " + LastNameInputTextBox.Input.Text;

            result += "\n\nPESEL: " + pesel + 
                      ", Data urodzenia: " + dateOfBirth + 
                      ", Płeć: " + sex;

            result += "\n\nWzrost: " + height + " cm, " +
                      "Masa: " + weight + " kg\n" +
                      "BMI: " + bmi +
                      ", Kategoria BMI: " + bmiCategory;

            BmiResultLabel.Content = result;
        }

        private Boolean ValidateInputs()
        {
            // Validate first name
            if (string.IsNullOrEmpty(FirstNameInputTextBox.Input.Text))
            {
                MessageBox.Show("Imię jest wymagane.", "Invalid input", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            // Validate last name
            if (string.IsNullOrEmpty(LastNameInputTextBox.Input.Text))
            {
                MessageBox.Show("Nazwisko jest wymagane.", "Invalid input", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            // Validate PESEL
            if (!Calculations.IsPeselValid(PeselInputTextBox.Input.Text))
            {
                MessageBox.Show("Numer PESEL jest nieprawidłowy.", "Invalid input", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            // Validate height
            if (string.IsNullOrEmpty(HeightInputTextBox.Input.Text))
            {
                MessageBox.Show("Wzrost jest wymagany.", "Invalid input", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            // Validate weight
            if (string.IsNullOrEmpty(WeightInputTextBox.Input.Text))
            {
                MessageBox.Show("Masa jest wymagana.", "Invalid input", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            // Validate height and weight as numbers
            if (!double.TryParse(HeightInputTextBox.Input.Text, out double height) || height <= 0)
            {
                MessageBox.Show("Wysokość musi być liczbą dodatnią.", "Invalid input", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (!double.TryParse(WeightInputTextBox.Input.Text, out double weight) || weight <= 0)
            {
                MessageBox.Show("Masa musi być liczbą dodatnią.", "Invalid input", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            return true;
        }
    }
}