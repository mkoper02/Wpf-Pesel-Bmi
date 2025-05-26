using System.ComponentModel;

namespace PeselBmiWpf
{
    public class Person : INotifyPropertyChanged
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Pesel { get; set; } = string.Empty;
        public double Height { get; set; } = 0;
        public double Weight { get; set; } = 0;
        public DateTime BirthDate => GetDateOfBirth();
        public char Sex => GetSex();
        public int Age => DateTime.Now.Year - BirthDate.Year - (DateTime.Now.DayOfYear < BirthDate.DayOfYear ? 1 : 0);
        public double Bmi => CalculateBmi();
        public string BmiCategory => GetBmiCategory();

        private char GetSex()
        {
            return Pesel[9] % 2 == 0 ? 'F' : 'M';
        }

        private DateTime GetDateOfBirth()
        {
            int year = int.Parse(Pesel.Substring(0, 2));
            int month = int.Parse(Pesel.Substring(2, 2));
            int day = int.Parse(Pesel.Substring(4, 2));
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

            return new DateTime(year + century, month, day);
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