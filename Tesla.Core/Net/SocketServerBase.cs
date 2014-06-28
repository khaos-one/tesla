using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace Tesla.Net
{
    public abstract class SocketServerBase<THandler>
        : ServerBase
    {
        protected Socket ListenerSocket;
        protected readonly THandler HandlerFunc;

        public IPEndPoint LocalEndPoint { get; protected set; }

        protected SocketServerBase(THandler handlerFunc, IPAddress ip, int port)
        {
            HandlerFunc = handlerFunc;
            LocalEndPoint = new IPEndPoint(ip, port);
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

        protected override void OnStart()
        {
            try
            {
                BindSocket();
            }
            catch (SocketException) { /* TODO: Process exception. */ }
            catch (ObjectDisposedException) { /* TODO: Process exception. */ }
        }

        protected override void OnStop()
        {
            ListenerSocket.Disconnect(true);
        }

        public new void Dispose()
        {
            base.Dispose();

            if (ListenerSocket != null)
            {
                ListenerSocket.Close();
                ListenerSocket.Dispose();
            }
        }
    }
}
