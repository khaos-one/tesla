using System;

namespace Tesla.Tests {
    public interface IStopwatch {
        bool IsRunning { get; }
        TimeSpan Elapsed { get; }

        void Start();
        void Stop();
        void Reset();
    }
}