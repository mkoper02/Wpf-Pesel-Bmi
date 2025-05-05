using PeselBmiWpf.Models;
using PeselBmiWpf.ViewModels;
using System.Windows;

namespace PeselBmiWpf.Views
{
    public partial class MainWindow : Window
    {
        private readonly string projectPath = AppContext.BaseDirectory.Substring(0, AppContext.BaseDirectory.IndexOf("bin"));
        private readonly string peopleFilePath;
        private readonly string bmiFilePath;

        public MainWindow()
        {
            InitializeComponent();

            peopleFilePath = string.Concat(projectPath, "Data\\people.csv");
            bmiFilePath = string.Concat(projectPath,"Data\\bmi.csv");

            // Set the DataContext to the MainViewModel
            var viewModel = new MainViewModel(peopleFilePath, bmiFilePath);
            DataContext = viewModel;

            // Subscribe to events
            InputPerson.PersonAdded += OnPersonAdded;
            InputBmiData.BmiDataAdded += OnBmiDataAdded;

            Closing += (s, e) =>
            {
                viewModel.SaveDataToCsv(peopleFilePath, bmiFilePath);
            };
        }

        private void OnPersonAdded(Person person)
        {
            var viewModel = DataContext as MainViewModel;
            viewModel?.People.Add(person);
        }

        private void OnBmiDataAdded(BmiRecord bmiRecord)
        {
            var viewModel = DataContext as MainViewModel;
            viewModel?.SelectedPerson?.BmiRecords.Add(bmiRecord);
        }

        private void DataGrid_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Delete)
            {
                var result = MessageBox.Show(
                    "Czy na pewno chcesz usunąć ten rekord?",
                    "Potwierdzenie usunięcia",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning);

                // If the user clicks "No", cancel the deletion
                if (result != MessageBoxResult.Yes)
                {
                    e.Handled = true; // Prevent the default delete action
                }
            }
        }
    }
}
