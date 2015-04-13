using System.Linq;
using System.Text.RegularExpressions;

namespace Generator.Nip
{
    public class NipValidator
    {
        public static bool IsValid(string nip)
        {         
            var regex = new Regex("^\\d{10}$");
            if (!regex.IsMatch(nip))
            {
                return false;
            }

            int checkSum = NipCheckSumCalculator.Calculate(nip);

            if (nip.Last() - '0' != checkSum)
            {
                return false;
            }
            
            return TaxOffice.Codes.Contains(nip.Substring(0, 3));
        }
    }
}
