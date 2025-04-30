using PeselBmiWpf.Models;
using PeselBmiWpf.ViewModels;
using System.Windows;

namespace PeselBmiWpf.Views
{
    public partial class MainWindow : Window
    {
        private readonly string filePath = string.Concat(AppContext.BaseDirectory.AsSpan(0, AppContext.BaseDirectory.IndexOf("bin")), "\\dane.json");

        public MainWindow()
        {
            InitializeComponent();
            
            var viewModel = new MainViewModel(filePath);
            DataContext = viewModel;

            InputPerson.PersonAdded += OnPersonAdded;
            InputBmiData.BmiDataAdded += OnBmiDataAdded;

            Closing += (s, e) =>
            {
                viewModel.SaveDataToJson(filePath);
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
            if (viewModel?.SelectedPerson != null)
            {
                viewModel.SelectedPerson.BmiRecords.Add(bmiRecord);
            }
        }
    }
}
