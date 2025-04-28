using System.Text.RegularExpressions;

namespace PeselBmiWpf
{
    static class Calculations
    {
        // ***************************************************************
        // PESEL
        // ***************************************************************
        public static Boolean IsPeselValid(string pesel)
        {
            return ExtractDateOfBirth(pesel) != null;
        }
        private static bool IsTextNumeric(string text) => Regex.IsMatch(text, "^[0-9]+$");

        private static DateTime? ExtractDateOfBirth(string pesel)
        {
            // Validate PESEL length and numeric content
            if (string.IsNullOrEmpty(pesel) || pesel.Length != 11 || !IsTextNumeric(pesel))
            {
                return null;
            }

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
                return new DateTime(year, month, day, 0, 0, 0, DateTimeKind.Unspecified);
            }
            catch
            {
                return null; // Invalid date
            }
        }

        public static string? GetDateOfBirth(string pesel)
        {
            DateTime? birthDate = ExtractDateOfBirth(pesel);
            return birthDate?.ToString("dd.MM.yyyy") ?? "Nieprawidłowy PESEL";
        }

        public static Boolean IsMale(string pesel)
        {
            return pesel[10] % 2 == 0;
        }


        // ***************************************************************
        // BMI
        // ***************************************************************
        public static double CalculateBmi(double weight, double height)
        {
            // Convert height from cm to meters
            height = height / 100;

            return Math.Round(weight / (height * height), 2);
        }

        public static string GetBmiCategory(double bmi)
        {
            if (bmi < 18.5)
            {
                return "Niedowaga";
            }
            else if (bmi < 24.9)
            {
                return "Waga normalna";
            }
            else if (bmi < 29.9)
            {
                return "Nadwaga";
            }
            else
            {
                return "Otyłość";
            }
        }
    }
}
