using PeselBmiWpf.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Windows;

namespace PeselBmiWpf.ViewModels;
public class MainViewModel : INotifyPropertyChanged
{
    public ObservableCollection<Person> People { get; set; } = [];
    public ObservableCollection<BmiRecord> SelectedBmiHistory => SelectedPerson?.BmiRecords ?? [];

    private Person? _selectedPerson;
    public Person? SelectedPerson
    {
        get => _selectedPerson;
        set
        {
            _selectedPerson = value;
            OnPropertyChanged(nameof(SelectedPerson));
            OnPropertyChanged(nameof(SelectedBmiHistory));
        }
    }

    private BmiRecord? _selectedBmiRecord;
    public BmiRecord? SelectedBmiRecord
    {
        get => _selectedBmiRecord;
        set
        {
            _selectedBmiRecord = value;
            OnPropertyChanged(nameof(SelectedBmiRecord));
        }
    }

    public MainViewModel(string peopleFilePath, string bmiFilePath)
    {
        LoadDataFromCsv(peopleFilePath, bmiFilePath);
    }

    public void LoadDataFromCsv(string peopleFilePath, string bmiFilePath)
    {
        if (!File.Exists(peopleFilePath))
        {
            MessageBox.Show($"Nie znaleziono pliku z danymi: {peopleFilePath}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        if (!File.Exists(bmiFilePath))
        {
            MessageBox.Show($"Nie znaleziono pliku z danymi: {bmiFilePath}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        try
        {
            // People
            using (var reader = new StreamReader(peopleFilePath))
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
                        Pesel = columns[2][..11]
                    };

                    People.Add(person);
                }
            }

            // Bmi
            using (var reader = new StreamReader(bmiFilePath))
            {
                var lines = reader.ReadToEnd().Split('\n').Skip(1); // Skip header line
                foreach (var line in lines)
                {
                    if (string.IsNullOrWhiteSpace(line))
                    {
                        continue;
                    }

                    var columns = line.Split(',');
                    var bmiRecord = new BmiRecord
                    {
                        Weight = double.Parse(columns[1]),
                        Height = double.Parse(columns[2]),
                        Date = DateTime.Parse(columns[3])
                    };

                    // Find the person by PESEL and add the BMI record
                    var pesel = columns[0];
                    var person = People.FirstOrDefault(p => p.Pesel == pesel);

                    person?.BmiRecords.Add(bmiRecord);
                }
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Błąd podczas wczytywania danych: {ex.Message}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    public void SaveDataToCsv(string peopleFilePath, string bmiFilePath)
    {
        try
        {
            // People
            using (var writer = new StreamWriter(peopleFilePath))
            {
                writer.WriteLine("Imię,Nazwisko,PESEL");

                foreach (var person in People)
                {
                    writer.WriteLine($"{person.FirstName},{person.LastName},{person.Pesel}");
                }
            }

            // Bmi
            using (var writer = new StreamWriter(bmiFilePath))
            {
                writer.WriteLine("PESEL,Masa,Wzrost,Data");
                foreach (var person in People)
                {
                    foreach (var bmiRecord in person.BmiRecords)
                    {
                        writer.WriteLine($"{person.Pesel},{bmiRecord.Weight},{bmiRecord.Height},{bmiRecord.Date}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Błąd podczas zapisywania danych: {ex.Message}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged(string prop) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
}
