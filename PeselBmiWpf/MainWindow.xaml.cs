using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace PeselBmiWpf
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private readonly string filePath = string.Concat(AppContext.BaseDirectory.Substring(0, AppContext.BaseDirectory.IndexOf("bin")), "data.csv");
        public event Action<Person> PersonAdded;

        public ObservableCollection<Person> People { get; set; } = [];
        private Person? _selectedPerson;
        public Person? SelectedPerson
        {
            get => _selectedPerson;
            set
            {
                _selectedPerson = value;
                OnPropertyChanged(nameof(SelectedPerson));
            }
        }

        public MainWindow()
        {
            InitializeComponent();

            // Load data from CSV file
            LoadDataFromCsv();

            // Set DataContext for data binding
            DataContext = this;

            // Subscribe to event
            PersonAdded += OnPersonAdded;

            Closing += (s, e) =>
            {
                SaveDataToCsv();
            };
        }

        private void OnPersonAdded(Person person)
        {
            People.Add(person);
        }

        private void DataGrid_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Delete)
            {
                var result = MessageBox.Show("Czy na pewno chcesz usunąć ten rekord?", "Potwierdzenie usunięcia", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (result != MessageBoxResult.Yes)
                {
                    e.Handled = true; // Prevent the default delete action
                }
            }
        }

        private void AddPersonButton_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateInput())
            {
                return;
            }

            var person = new Person
            {
                FirstName = FirstNameTextBox.Text,
                LastName = LastNameTextBox.Text,
                Pesel = PeselTextBox.Text,
                Height = double.Parse(HeightTextBox.Text),
                Weight = double.Parse(WeightTextBox.Text)
            };

            // Raise the PersonAdded event
            PersonAdded.Invoke(person);
            ClearPersonInputTextBox();
        }

        private void UpdatePersonButton_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateInput())
            {
                return;
            }

            // Update the binding source
            var firstNameBinding = FirstNameTextBox.GetBindingExpression(TextBox.TextProperty);
            var lastNameBinding = LastNameTextBox.GetBindingExpression(TextBox.TextProperty);
            var peselBinding = PeselTextBox.GetBindingExpression(TextBox.TextProperty);
            var heightBinding = HeightTextBox.GetBindingExpression(TextBox.TextProperty);
            var weightBinding = WeightTextBox.GetBindingExpression(TextBox.TextProperty);

            firstNameBinding.UpdateSource();
            lastNameBinding.UpdateSource();
            peselBinding.UpdateSource();
            heightBinding.UpdateSource();
            weightBinding.UpdateSource();

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
            if (string.IsNullOrWhiteSpace(FirstNameTextBox.Text) ||
                string.IsNullOrWhiteSpace(LastNameTextBox.Text) ||
                string.IsNullOrWhiteSpace(PeselTextBox.Text) ||
                string.IsNullOrEmpty(HeightTextBox.Text) ||
                string.IsNullOrEmpty(WeightTextBox.Text))
            {
                MessageBox.Show("Wszystkie pola są wymagane.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (!Person.IsPeselValid2(PeselTextBox.Text))
            {
                MessageBox.Show("Pesel jest niepoprawny.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (!double.TryParse(HeightTextBox.Text, out double height) || height <= 0)
            {
                MessageBox.Show("Wzrost musi być liczbą dodatnią.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (!double.TryParse(WeightTextBox.Text, out double weight) || weight <= 0)
            {
                MessageBox.Show("Waga musi być liczbą dodatnią.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            return true;
        }

        private void ClearPersonInputTextBox()
        {
            FirstNameTextBox.Clear();
            LastNameTextBox.Clear();
            PeselTextBox.Clear();
            HeightTextBox.Clear();
            WeightTextBox.Clear();
        }

        private void LoadDataFromCsv()
        {
            if (!File.Exists(filePath))
            {
                MessageBox.Show($"Nie znaleziono pliku z danymi: {filePath}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                using (var reader = new StreamReader(filePath))
                {
                    var lines = reader.ReadToEnd().Split('\n').Skip(1); // Skip header line

                    foreach (var line in lines)
                    {
                        if (string.IsNullOrWhiteSpace(line))
                        {
                            continue;
                        }

                        var columns = line.Split(',');
                        var person = new Person
                        {
                            FirstName = columns[0],
                            LastName = columns[1],
                            Pesel = columns[2],
                            Height = double.Parse(columns[3]),
                            Weight = double.Parse(columns[4]),
                        };

                        People.Add(person);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Błąd podczas wczytywania danych: {ex.Message}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void SaveDataToCsv()
        {
            try
            {
                using (var writer = new StreamWriter(filePath))
                {
                    writer.WriteLine("Imię,Nazwisko,PESEL,Wzrost,Waga");
                    foreach (var person in People)
                    {
                        writer.WriteLine($"{person.FirstName},{person.LastName},{person.Pesel},{person.Height},{person.Weight}");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Błąd podczas zapisywania danych: {ex.Message}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

}
