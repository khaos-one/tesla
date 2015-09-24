using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tesla.Protocol.Tests {
    [TestClass]
    public class TypeDiscoveryTests {
        [TestMethod]
        public void SubclassFindTest() {
            var types = TypeDiscovery.FindSubclassTypes<ClassA>();
        }

        internal class ClassA {}

        internal class ClassB
            : ClassA {}

        internal class ClassC
            : ClassA {}
    }
}