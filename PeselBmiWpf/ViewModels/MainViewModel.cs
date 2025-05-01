using PeselBmiWpf.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Text.Json;
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

    public MainViewModel(string filePath)
    {
        LoadDataFromJson(filePath);
    }

    private void LoadDataFromJson(string filePath)
    {
        if (File.Exists(filePath))
        {
            try
            {
                string json = File.ReadAllText(filePath);
                var people = JsonSerializer.Deserialize<ObservableCollection<Person>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (people != null)
                {
                    foreach (var person in people)
                    {
                        People.Add(person);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Błąd podczas wczytywania danych: {ex.Message}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        else
        {
            MessageBox.Show($"Nie znaleziono pliku z danymi: {filePath}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
    }

    public void SaveDataToJson(string filePath)
    {
        try
        {
            string json = JsonSerializer.Serialize(People, new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            File.WriteAllText(filePath, json);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Błąd podczas zapisywania danych: {ex.Message}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged(string prop) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
}
