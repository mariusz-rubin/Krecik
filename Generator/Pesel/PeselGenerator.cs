using System;
using System.Text;

namespace Generator.Pesel
{
    public class PeselGenerator
    {
        private readonly Random _random;

        public PeselGenerator()
        {
            _random = new Random();
        }

        public string Generate()
        {
            var peselStringBuilder = new StringBuilder();
            DateTime birthDate = GenerateDate(1900, 2099);

            AppendPeselDate(birthDate, peselStringBuilder);

            peselStringBuilder.Append(GenerateRandomNumbers(4));

            peselStringBuilder.Append(PeselCheckSumCalculator.Calculate(peselStringBuilder.ToString()));

            return peselStringBuilder.ToString();
        }       

        public static string GetPeselMonthShiftedByYear(DateTime date)
        {
            if(date.Year < 1900 || date.Year > 2299)
            {
                throw new NotSupportedException(string.Format("PESEL for year: {0} is not supported", date.Year));
            }

            int monthShift = (int)((date.Year - 1900) / 100) * 20;

            return (date.Month + monthShift).ToString("00");
        }

        private DateTime GenerateDate(int yearFrom, int yearTo)
        {
            int year = _random.Next(yearFrom, yearTo + 1);
            int month = _random.Next(12) + 1;
            int day = _random.Next(DateTime.DaysInMonth(year, month)) + 1;

            return new DateTime(year, month, day);
        }

        private void AppendPeselDate(DateTime date, StringBuilder builder)
        {
            builder.Append((date.Year % 100).ToString("00"));
            builder.Append(GetPeselMonthShiftedByYear(date));
            builder.Append(date.Day.ToString("00"));
        }

        private string GenerateRandomNumbers(int numbersCount)
        {
            int maxValue = (int)Math.Pow(10, numbersCount);
            string format = "D" + numbersCount;

            return _random.Next(maxValue).ToString(format);
        }
    }
}
