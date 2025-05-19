using PeselBmiWpf.Models;
using PeselBmiWpf.ViewModels;
using System.Windows;

namespace PeselBmiWpf.Views
{
    public partial class MainWindow : Window
    {
        private readonly string filePath = string.Concat(AppContext.BaseDirectory.Substring(0, AppContext.BaseDirectory.IndexOf("bin")), "Data\\data.csv");

        public MainWindow()
        {
            InitializeComponent();

            // Set the DataContext to the MainViewModel
            var viewModel = new MainViewModel(filePath);
            DataContext = viewModel;

            // Subscribe to event
            InputData.PersonAdded += OnPersonAdded;

            Closing += (s, e) =>
            {
                viewModel.SaveDataToCsv(filePath);
            };
        }

        private void OnPersonAdded(Person person)
        {
            var viewModel = DataContext as MainViewModel;
            viewModel?.People.Add(person);
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
    }
}
