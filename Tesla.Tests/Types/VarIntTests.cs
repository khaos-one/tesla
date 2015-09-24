using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tesla.Types;

namespace Tesla.Core.Tests.Types {
    [TestClass]
    public class VarIntTests {
        [TestMethod]
        public void VarIntPositiveNumbersToByteArrayAgainstSample() {
            // Found in specification by Google.
            var aSample = new byte[] {0x96, 0x01};
            var bSample = new byte[] {0xAC, 0x02};

            VarInt a = 150;
            VarInt b = 300;
            var aConverted = a.ToBytes().ToArray();
            var bConverted = b.ToBytes().ToArray();

            CollectionAssert.AreEqual(aSample, aConverted);
            CollectionAssert.AreEqual(bSample, bConverted);
        }

        [TestMethod]
        public void VarIntPositiveNumbersConversion() {
            VarInt a = 12345678901234567879;
            var b = a.ToBytes().ToArray();
            VarInt c = b;

            Assert.AreEqual(a, c);
        }

        [TestMethod]
        public void VarIntNegativeNumber() {
            VarInt a = -123;
            var b = a.ToByteArray();
            VarInt c = b;

            Assert.AreEqual(a, c);
        }
    }
}