using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace Tesla.Net
{
    public abstract class SocketServerBase<THandler>
        : IServer
    {
        private readonly CancellationTokenSource _cts;
        private int _started;
        private int _stopped;

        protected Socket ListenerSocket;
        protected readonly THandler HandlerFunc;

        public IPEndPoint LocalEndPoint { get; protected set; }

        protected SocketServerBase(THandler handlerFunc, IPAddress ip, int port)
        {
            HandlerFunc = handlerFunc;
            LocalEndPoint = new IPEndPoint(ip, port);
            _cts = new CancellationTokenSource();
        }

        protected SocketServerBase(THandler handlerFunc, int port)
            : this(handlerFunc, IPAddress.Any, port)
        { }

        protected SocketServerBase(THandler handlerFunc)
            : this(handlerFunc, 0)
        { }

        public int Port
        {
            get { return LocalEndPoint.Port; }
        }

        protected abstract void BindSocket();
        protected abstract Task<Action> AcceptClient();

        public void Start()
        {
            if (Interlocked.CompareExchange(ref _started, 1, 0) != 0)
            {
                throw new InvalidOperationException("Server has already been started.");
            }

            try
            {
                BindSocket();
            }
            catch (SocketException) { /* TODO: Process exception. */ }
            catch (ObjectDisposedException) { /* TODO: Process exception. */ }

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
                ListenerSocket.Disconnect(true);
            }
            catch (Exception e)
            {
                Trace.TraceWarning("TCP Server stop exception: {0}.", e.Message);
            }
        }

        public void Dispose()
        {
            Stop();

            if (ListenerSocket != null)
            {
                ListenerSocket.Close();
                ListenerSocket.Dispose();
            }
        }
    }
}
