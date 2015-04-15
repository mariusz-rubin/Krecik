using Krecik.Nip;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KrecikTests.Nip
{
    [TestClass]
    public class NipGeneratorTests
    {
        [TestMethod]
        public void Generate_builds_valid_NIP()
        {
            var nipGen = new NipGenerator();

            for (int i = 0; i < 100; i++)
            {
                string nip = nipGen.Generate();

                Assert.IsTrue(
                    NipValidator.IsValid(nip), 
                    string.Format("Failed nr: [{0}], nip: [{1}]", i, nip));
            }
        }
    }
}
