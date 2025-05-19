using System.ComponentModel;

namespace PeselBmiWpf.Models
{
    public class BmiRecord : INotifyPropertyChanged
    {
        private double _weight;
        public double Weight // in kg
        {
            get => _weight;
            set
            {
                _weight = value;
                OnPropertyChanged(nameof(Weight));
                OnPropertyChanged(nameof(Bmi));
                OnPropertyChanged(nameof(BmiCategory));
            }
        }

        private double _height;
        public double Height // in cm
        {
            get => _height;
            set
            {
                _height = value;
                OnPropertyChanged(nameof(Height));
                OnPropertyChanged(nameof(Bmi));
                OnPropertyChanged(nameof(BmiCategory));
            }
        }

        public DateTime Date { get; set; } = DateTime.Now;
        public double Bmi => CalculateBmi();
        public string BmiCategory => GetBmiCategory();

        // BMI = weight (kg) / (height (m) * height (m))
        private double CalculateBmi()
        {
            var height = Height / 100; // Convert height from cm to meters
            return Math.Round(Weight / (height * height), 2);
        }

        private string GetBmiCategory()
        {
            if (Bmi < 16)  return "Wygłodzenie";
            else if (Bmi < 17) return "Wychudzenie";
            else if (Bmi < 18.5) return "Niedowaga";
            else if (Bmi < 25) return "Waga prawidłowa";
            else if (Bmi < 30) return "Nadwaga";
            else if (Bmi < 35) return "Otyłość I stopnia";
            else if (Bmi < 40) return "Otyłość II stopnia";
            else return "Otyłość III stopnia";
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
