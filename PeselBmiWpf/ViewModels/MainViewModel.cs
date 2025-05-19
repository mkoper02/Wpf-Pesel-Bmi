using PeselBmiWpf.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Windows;

namespace PeselBmiWpf.ViewModels;
public class MainViewModel : INotifyPropertyChanged
{
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

    public MainViewModel(string filePath)
    {
        LoadDataFromCsv(filePath);
    }

    private void LoadDataFromCsv(string filePath)
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

    public void SaveDataToCsv(string filePath)
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
