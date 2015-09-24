using System;
using System.Diagnostics;
using System.Threading;

namespace Tesla.Tests {
    public class Timewatch
        : Stopwatch,
            IStopwatch {
        public Timewatch() {
            if (!IsHighResolution) {
                throw new NotSupportedException("Your hardware does not support high resolution timers.");
            }

            var seed = Environment.TickCount;
            Process.GetCurrentProcess().ProcessorAffinity = new IntPtr(2);
            Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.High;
            Thread.CurrentThread.Priority = ThreadPriority.Highest;
        }
    }
}