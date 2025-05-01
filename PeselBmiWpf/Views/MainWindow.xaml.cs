using PeselBmiWpf.Models;
using PeselBmiWpf.ViewModels;
using System.Windows;

namespace PeselBmiWpf.Views
{
    public partial class MainWindow : Window
    {
        private readonly string filePath = string.Concat(
            AppContext.BaseDirectory.AsSpan(0, AppContext.BaseDirectory.IndexOf("bin")),
            "\\dane.json"
        );

        public MainWindow()
        {
            InitializeComponent();

            // Set the DataContext to the MainViewModel
            var viewModel = new MainViewModel(filePath);
            DataContext = viewModel;

            // Subscribe to events
            InputPerson.PersonAdded += OnPersonAdded;
            InputBmiData.BmiDataAdded += OnBmiDataAdded;

            // Save data to JSON when the window is closing
            Closing += (s, e) =>
            {
                viewModel.SaveDataToJson(filePath);
            };
        }

        private void OnPersonAdded(Person person)
        {
            // Add the new person to the People collection in the ViewModel
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
