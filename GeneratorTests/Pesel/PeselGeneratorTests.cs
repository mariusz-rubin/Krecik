using Generator.Pesel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneratorTests.Pesel
{
    [TestClass]
    public class PeselGeneratorTests
    {
        [TestMethod]
        public void GetPeselMonthShiftedByYear_gets_month_for_PESEL_according_to_year()
        {
            var testCaseData = new[] {
                new { Date = new DateTime(1900, 1, 1), ExpectedMonth = "01" },
                new { Date = new DateTime(1999, 1, 1), ExpectedMonth = "01" },
                new { Date = new DateTime(2000, 1, 1), ExpectedMonth = "21" },
                new { Date = new DateTime(2099, 1, 1), ExpectedMonth = "21" },
                new { Date = new DateTime(2100, 1, 1), ExpectedMonth = "41" },
                new { Date = new DateTime(2199, 1, 1), ExpectedMonth = "41" },
                new { Date = new DateTime(2200, 1, 1), ExpectedMonth = "61" },
                new { Date = new DateTime(2299, 1, 1), ExpectedMonth = "61" },
            };

            foreach(var testCase in testCaseData)
            {
                string month = PeselGenerator.GetPeselMonthShiftedByYear(testCase.Date);

                Assert.AreEqual(
                    testCase.ExpectedMonth,
                    month,
                    string.Format("Failed for date: [{0}], expected month: [{1}] but was [{2}]", testCase.Date, testCase.ExpectedMonth, month));
            }
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void GetPeselMonthShiftedByYear_is_supported_from_year_1900()
        {
            PeselGenerator.GetPeselMonthShiftedByYear(new DateTime(1899, 1, 1));
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void GetPeselMonthShiftedByYear_is_supported_to_year_2299()
        {
            PeselGenerator.GetPeselMonthShiftedByYear(new DateTime(2300, 1, 1));
        }
        
        [TestMethod]
        public void Generate_builds_valid_PESEL()
        {
            var peselGenerator = new PeselGenerator();

            for (int i = 0; i < 100; i++)
            {
                string pesel = peselGenerator.Generate();

                Assert.IsTrue(
                    PeselValidator.IsValid(pesel), 
                    string.Format("Failed nr: [{0}], pesel: [{1}]", i, pesel));
            }            
        }   
    }
}
