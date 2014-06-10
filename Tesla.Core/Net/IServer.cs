using System;

namespace Tesla.Net
{
    public interface IServer
        : IDisposable
    {
        void Start();
        void Stop();
    }
}
