using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tesla.Protocol.Tests {
    [TestClass]
    public class ObjectMarshalTests {
        [TestMethod]
        public void SampleObjectRoundtripTest() {
            var sample = new SampleStruct {
                UInt32Val = 72632,
                Int64Val = -726253,
                FloatVal = 3.1426f,
                StringVal = "I see dead people",
                Int32Array = new[] {2, 4, 6, 8, 10, 12, 14, 16, 18, 20},
                StringArray = new[] {"I see", "dead people", "and", "shit", "brix"},
                DoubleVal = 743.76236
            };
            var data = ObjectMarshal.BytesFromStructure(sample);
            var deser = ObjectMarshal.StructureFromBytes<SampleStruct>(data);

            Assert.AreEqual(sample.UInt32Val, deser.UInt32Val);
            Assert.AreEqual(sample.Int64Val, deser.Int64Val);
            Assert.AreEqual(sample.FloatVal, deser.FloatVal);
            Assert.AreEqual(sample.StringVal, deser.StringVal);
            CollectionAssert.AreEqual(sample.Int32Array, deser.Int32Array);
            CollectionAssert.AreEqual(sample.StringArray, deser.StringArray);
            Assert.AreEqual(sample.DoubleVal, deser.DoubleVal);
        }

        private struct SampleStruct {
            public uint UInt32Val;
            public long Int64Val;
            public float FloatVal;
            public string StringVal;
            public int[] Int32Array;
            public string[] StringArray;
            public double DoubleVal;
        }
    }
}