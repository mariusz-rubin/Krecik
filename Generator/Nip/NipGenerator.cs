using System;
using System.Text;

namespace Generator.Nip
{
    public class NipGenerator
    {
        private readonly Random _random;

        public NipGenerator()
        {
            _random = new Random();
        }

        public string Generate()
        {
            var nipNumberBuilder = new StringBuilder();

            string taxOfficePrefix = TaxOffice.Codes[_random.Next(TaxOffice.Codes.Length)];

            nipNumberBuilder.Append(taxOfficePrefix);

            nipNumberBuilder.Append(GenerateRandomNumbers(6));

            int checksum = NipCheckSumCalculator.Calculate(nipNumberBuilder.ToString());

            while (checksum == 10)
            {
                // change last number, check sum must be different from 10
                nipNumberBuilder.Remove(nipNumberBuilder.Length - 1, 1);
                nipNumberBuilder.Append(_random.Next(10).ToString());

                checksum = NipCheckSumCalculator.Calculate(nipNumberBuilder.ToString());
            }

            nipNumberBuilder.Append(checksum);

            return nipNumberBuilder.ToString();
        }

        private string GenerateRandomNumbers(int numbersCount)
        {
            int maxValue = (int)Math.Pow(10, numbersCount);
            string format = "D" + numbersCount;

            return _random.Next(maxValue).ToString(format);
        }
    }
}
