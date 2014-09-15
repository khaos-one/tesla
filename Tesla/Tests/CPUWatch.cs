using System;
using System.Diagnostics;

namespace Tesla.Tests
{
    public class CpuWatch
        : IStopwatch
    {
        protected TimeSpan StartTime;
        protected TimeSpan StopTime;
        protected bool IsStopwatchRunning;

        public bool IsRunning
        {
            get { return IsStopwatchRunning; }
        }

        public TimeSpan Elapsed
        {
            get
            {
                if (IsRunning)
                {
                    throw new InvalidOperationException("Cannot get elapsed time while stopwatch is still running.");
                }

                return StopTime - StartTime;
            }
        }

        public void Start()
        {
            StartTime = Process.GetCurrentProcess().TotalProcessorTime;
            IsStopwatchRunning = true;
        }

        public void Stop()
        {
            StopTime = Process.GetCurrentProcess().TotalProcessorTime;
            IsStopwatchRunning = false;
        }

        public void Reset()
        {
            StartTime = StopTime = TimeSpan.Zero;
            IsStopwatchRunning = false;
        }
    }
}
