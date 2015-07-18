using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tesla.Tests
{
    [TestClass]
    public sealed class StringExtensionsTests
    {
        [TestMethod]
        public void SplitWildcardTest()
        {
            const string str = "iseedeadpeopleeverywherenear";
            const string pattern = "isee*people*near";
            var sampleResult = new[] {"dead", "everywhere"};
            var result = str.SplitWildcard(pattern);

            CollectionAssert.AreEqual(sampleResult, result);
        }

        private static readonly string str = "iseedeadpeopleeverywherenear";
        private static readonly string pattern = "isee*people*near";
        private static readonly Regex SplitWildcardBenchmarkAction2Regex = new Regex("isee(.*?)people(.*?)near");

        private static void SplitWildcardBenchmarkAction2()
        {
            SplitWildcardBenchmarkAction2Regex.GetMatchingGroups(str);
        }

        private static void SplitWildcardBenchmarkAction1()
        {
            str.SplitWildcard(pattern);
        }

        [TestMethod]
        public void SplitWildcardBenchmark()
        {
            var bn1 = Benchmark.Run<Timewatch>(SplitWildcardBenchmarkAction1, 10000);
            var mean1 = bn1.Mean();
            var bn2 = Benchmark.Run<Timewatch>(SplitWildcardBenchmarkAction2, 10000);
            var mean2 = bn2.Mean();
        }
    }
}
