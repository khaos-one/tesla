using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Tesla.Net
{
    public abstract class ServerBase 
        : IServer
    {
        private readonly CancellationTokenSource _cts = new CancellationTokenSource();
        private int _started;
        private int _stopped;

        protected abstract void OnStart();
        protected abstract void OnStop();
        protected abstract Task<Action> AcceptClient();

        public void Start()
        {
            if (Interlocked.CompareExchange(ref _started, 1, 0) != 0)
            {
                throw new InvalidOperationException("Server has already been started.");
            }

            OnStart();

            #pragma warning disable 4014
            ListenAsync();
            #pragma warning restore 4014
        }

        protected async Task ListenAsync()
        {
            if (_cts.Token.IsCancellationRequested)
            {
                return;
            }

            var accept = await AcceptClient();
            ThreadPool.QueueUserWorkItem(_ => accept());

            await ListenAsync();
        }

        public void Stop()
        {
            if (_started == 0)
            {
                return;
            }

            if (Interlocked.CompareExchange(ref _stopped, 1, 0) != 0)
            {
                return;
            }

            try
            {
                _cts.Cancel();
                OnStop();
            }
            catch (Exception e)
            {
                Trace.TraceWarning("Server stop exception: {0}.", e);
            }
        }

        public void Dispose()
        {
            Stop();
        }
    }
}
