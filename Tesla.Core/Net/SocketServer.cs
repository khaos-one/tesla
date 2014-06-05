using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace Tesla.Net
{
    using HandlerFunc = Func<Socket, Task>;

    public class SocketServer
        : IDisposable
    {
        private readonly HandlerFunc _handler;
        private readonly CancellationTokenSource _cts;
        private readonly Socket _listenerSocket;
        
        private IPEndPoint _localEndPoint;

        private int _started;
        private int _stopped;

        public SocketServer(HandlerFunc handler, IPAddress ip, int port)
        {
            _handler = handler;
            _localEndPoint = new IPEndPoint(ip, port);
            _cts = new CancellationTokenSource();

            _listenerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _listenerSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);
            _listenerSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.DontLinger, true);
        }

        public SocketServer(HandlerFunc handler, int port)
            : this(handler, IPAddress.Any, port)
        { }

        public IPEndPoint LocalEndPoint
        {
            get { return _localEndPoint; }
        }

        public int Port
        {
            get { return _localEndPoint.Port; }
        }

        public void Start()
        {
            if (Interlocked.CompareExchange(ref _started, 1, 0) != 0)
            {
                throw new InvalidOperationException("Server has already started.");
            }

            try
            {
                _listenerSocket.Bind(_localEndPoint);
                _listenerSocket.Listen(500);

                // Need to reassign endpoint to reflect dynamic IP/port allocation if any.
                _localEndPoint = (IPEndPoint) _listenerSocket.LocalEndPoint;
            }
            catch (SocketException) { /* TODO: Process exception. */ }
            catch (ObjectDisposedException) { /* TODO: Process exception. */ }

#pragma warning disable 4014
            ListenAsync();
#pragma warning restore 4014

            Trace.TraceInformation("TCP Server is listening on local endpoint {0}.", _localEndPoint);
        }

        private async Task ListenAsync()
        {
            if (_cts.Token.IsCancellationRequested)
            {
                return;
            }

            var socket = await _listenerSocket.AcceptAsync();

            Action accept = async () =>
            {
                if (!socket.Connected)
                {
                    Disconnect(socket);
                    return;
                }
                
                try
                {
                    await _handler(socket);
                }
                catch (Exception e)
                {
                    Trace.TraceWarning("TCP Handler exception: {0}.", e.Message);
                }

                Disconnect(socket);
            };

            ThreadPool.QueueUserWorkItem(state =>
            {
                Trace.TraceInformation("TCP client accepted: {0}.", (IPEndPoint)socket.RemoteEndPoint);
                accept();
            });

            await ListenAsync();
        }

        private static void Disconnect(Socket socket, Stream stream = null)
        {
            try
            {
                if (stream != null)
                {
                    stream.Close();
                }

                if (socket != null)
                {
                    socket.Shutdown(SocketShutdown.Both);
                    socket.Close();
                }
            }
            catch (Exception e)
            {
                Trace.TraceWarning("TCP closure exception: {0}.", e.Message);
            }
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
                _listenerSocket.Disconnect(true);
            }
            catch (Exception e)
            {
                Trace.TraceWarning("TCP Server stop exception: {0}.", e.Message);
            }
        }

        public void Dispose()
        {
            Stop();

            if (_listenerSocket != null)
            {
                _listenerSocket.Close();
                _listenerSocket.Dispose();
            }
        }
    }
}
