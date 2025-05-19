using System.ComponentModel;

namespace PeselBmiWpf.Models
{
    public class Person : INotifyPropertyChanged
    {
        private string _firstName = string.Empty;
        public string FirstName
        {
            get => _firstName;
            set
            {
                _firstName = value;
                OnPropertyChanged(nameof(FirstName));
            }
        }

        private string _lastName = string.Empty;
        public string LastName
        {
            get => _lastName;
            set
            {
                _lastName = value;
                OnPropertyChanged(nameof(LastName));
            }
        }

        private string _pesel = string.Empty;
        public string Pesel
        {
            get => _pesel;
            set
            {
                _pesel = value;
                OnPropertyChanged(nameof(Pesel));
                OnPropertyChanged(nameof(BirthDate));
                OnPropertyChanged(nameof(Sex));
                OnPropertyChanged(nameof(Age));
            }
        }

        private double _height;
        public double Height
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

        private double _weight;
        public double Weight
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

        public DateOnly BirthDate => ExtractDateOfBirth(Pesel) ?? new DateOnly(0, 0, 0);
        public char Sex => GetSex(Pesel);
        public int Age => DateTime.Now.Year - BirthDate.Year - (DateTime.Now.DayOfYear < BirthDate.DayOfYear ? 1 : 0);
        public double Bmi => CalculateBmi();
        public string BmiCategory => GetBmiCategory();

        private static char GetSex(string pesel)
        {
            return (pesel[9] % 2 == 0) ? 'F' : 'M';
        }

        private static DateOnly? ExtractDateOfBirth(string pesel)
        {
            int year = int.Parse(pesel.Substring(0, 2));
            int month = int.Parse(pesel.Substring(2, 2));
            int day = int.Parse(pesel.Substring(4, 2));
            int century;

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
                return null; // Invalid month
            }

            year += century;

            try
            {
                return new DateOnly(year, month, day);
            }
            catch
            {
                return null; // Invalid date
            }
        }      

        public static bool IsPeselValid(string pesel)
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

            if (ExtractDateOfBirth(pesel) is null)
            {
                return false;
            }

            return true;
        }

        // BMI = weight (kg) / (height (m) * height (m))
        private double CalculateBmi()
        {
            var heightInMeters = Height / 100;
            return Math.Round(Weight / (heightInMeters * heightInMeters), 2);
        }

        private string GetBmiCategory()
        {
            if (Bmi < 16) return "Wygłodzenie";
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
