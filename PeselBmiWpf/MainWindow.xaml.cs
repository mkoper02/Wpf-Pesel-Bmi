using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Windows;

namespace PeselBmiWpf
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private readonly string filePath = string.Concat(AppContext.BaseDirectory.Substring(0, AppContext.BaseDirectory.IndexOf("bin")), "data.csv");

        public ObservableCollection<Person> People { get; set; } = [];

        public MainWindow()
        {
            InitializeComponent();

            // Load data from CSV file
            LoadDataFromCsv();

            // Set DataContext for data binding
            DataContext = this;

            // When the window is closing, save data to CSV file
            Closing += (s, e) =>
            {
                SaveDataToCsv();
            };
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

            People.Add(person);

            // Notify the UI that the People collection has changed
            OnPropertyChanged(nameof(People));

            FirstNameTextBox.Clear();
            LastNameTextBox.Clear();
            PeselTextBox.Clear();
            HeightTextBox.Clear();
            WeightTextBox.Clear();
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

            if (!IsPeselValid(PeselTextBox.Text))
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

        private bool IsPeselValid(string pesel)
        {
            if (pesel.Length != 11)
            {
                return false;
            }

            // Check if pesel contains only digits
            if (!long.TryParse(pesel, out _))
            {
                return false;
            }

            int year = int.Parse(pesel.Substring(0, 2));
            int month = int.Parse(pesel.Substring(2, 2));
            int day = int.Parse(pesel.Substring(4, 2));
            int century = 0;

            if (month >= 1 && month <= 12)
            {
                century = 1900;
            }
            else if (month >= 21 && month <= 32)
            {
                month -= 20;
                century = 2000;
            }
            else if (month >= 41 && month <= 52)
            {
                month -= 40;
                century = 2100;
            }
            else if (month >= 61 && month <= 72)
            {
                month -= 60;
                century = 2200;
            }
            else if (month >= 81 && month <= 92)
            {
                month -= 80;
                century = 1800;
            }
            else
            {
                return false; // Invalid month
            }

            if (!DateTime.TryParse($"{year + century}-{month:D2}-{day:D2}", out DateTime _))
            {
                return false;
            }

            return true;
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
