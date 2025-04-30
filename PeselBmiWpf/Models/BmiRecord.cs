using System.Text.Json.Serialization;

namespace PeselBmiWpf.Models
{
    public class BmiRecord
    {
        public DateTime Date { get; set; } = DateTime.Now;
        public double Weight { get; set; } // in kg
        public double Height { get; set; } // in cm

        [JsonIgnore]
        public double Bmi => CalculateBmi();
        
        [JsonIgnore]
        public string BmiCategory => GetBmiCategory(Bmi);


        // BMI = weight (kg) / (height (m) * height (m))
        private double CalculateBmi()
        {
            var height = Height / 100; // Convert height from cm to meters
            return Math.Round(Weight / (height * height), 2);
        }

        private static string GetBmiCategory(double bmi)
        {
            if (bmi < 16)  return "Wygłodzenie";
            else if (bmi < 17) return "Wychudzenie";
            else if (bmi < 18.5) return "Niedowaga";
            else if (bmi < 25) return "Waga prawidłowa";
            else if (bmi < 30) return "Nadwaga";
            else if (bmi < 35) return "Otyłość I stopnia";
            else if (bmi < 40) return "Otyłość II stopnia";
            else return "Otyłość III stopnia";
        }
    }
}
