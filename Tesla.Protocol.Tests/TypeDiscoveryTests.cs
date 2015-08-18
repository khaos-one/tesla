using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tesla.Protocol.Tests
{
    [TestClass]
    public class TypeDiscoveryTests
    {
        internal class ClassA
        { }

        internal class ClassB
            : ClassA
        { }

        internal class ClassC
            : ClassA
        { }

        [TestMethod]
        public void SubclassFindTest()
        {
            var types = TypeDiscovery.FindSubclassTypes<ClassA>();
            
        }
    }
}
