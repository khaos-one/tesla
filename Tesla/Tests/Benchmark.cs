using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tesla.Tests
{
    public static class Benchmark
    {
        public static IList<double> Run<T>(Action action, uint iterations, uint runs = 5)
            where T : IStopwatch, new()
        {
            // Collect garbage and force pending finalizations.
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            // Warming run.
            action();

            var stopwatch = new T();
            var timings = new double[runs];

            for (var i = 0; i < timings.Length; i++)
            {
                stopwatch.Reset();
                stopwatch.Start();

                for (var j = 0; j < iterations; j++)
                {
                    action();
                }

                stopwatch.Stop();

                timings[i] = stopwatch.Elapsed.TotalMilliseconds;
            }

            return timings;
        }
    }
}
