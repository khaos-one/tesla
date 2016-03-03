using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tesla.Extensions;

namespace Tesla.Tests {
    [TestClass]
    public sealed class StringExtensionsTests {
        private static readonly string str = "iseedeadpeopleeverywherenear";
        private static readonly string pattern = "isee*people*near";
        private static readonly Regex SplitWildcardBenchmarkAction2Regex = new Regex("isee(.*?)people(.*?)near", RegexOptions.Compiled);

        [TestMethod]
        public void SplitWildcardTest() {
            var str = new [] {
                "iseedeadpeopleeverywherenear",
                "/",
                "",
                ""
            };
            var pattern = new[] {
                "isee*people*near",
                "/",
                "",
                "/"
            };
            var sampleResult = new List<string[]> {
                new[] {"dead", "everywhere"},
                new string[] {},
                null
            };

            for (var i = 0; i < str.Length; i++) {
                var result = str[i].SplitWildcard(pattern[i]);

                CollectionAssert.AreEqual(sampleResult[i], result);
            }

            
        }

        private static void SplitWildcardBenchmarkAction2() {
            SplitWildcardBenchmarkAction2Regex.GetMatchingGroups(str);
        }

        private static void SplitWildcardBenchmarkAction1() {
            str.SplitWildcard(pattern);
        }

        [TestMethod]
        public void SplitWildcardBenchmark() {
            var bn1 = Benchmark.Run<Timewatch>(SplitWildcardBenchmarkAction1, 10000);
            var mean1 = bn1.Mean();
            var bn2 = Benchmark.Run<Timewatch>(SplitWildcardBenchmarkAction2, 10000);
            var mean2 = bn2.Mean();
        }

        [TestMethod]
        public void SplitWildcardUnsafeBenchmark() {
            var bn1 = Benchmark.Run<Timewatch>(SplitWildcardBenchmarkAction1, 10000);
            var mean1 = bn1.Mean();
            var bn2 = Benchmark.Run<Timewatch>(SplitWildcardBenchmarkAction2, 10000);
            var mean2 = bn2.Mean();
        }
    }
}