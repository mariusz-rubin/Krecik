using System.Linq;
using System.Text.RegularExpressions;

namespace Generator.Pesel
{
    public class PeselValidator
    {
        public static bool IsValid(string pesel)
        {
            var regex = new Regex("^\\d{11}$");

            if (!regex.IsMatch(pesel))
            {
                return false;
            }

            int checkSum = PeselCheckSumCalculator.Calculate(pesel);
            int lastDigit = pesel.Last() - '0';

            return lastDigit == checkSum;
        }
    }
}
