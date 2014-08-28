using System;
using System.Net;
using System.Net.Sockets;

namespace Tesla.Net
{
    public abstract class SocketServerBase<THandler, TExceptionHandler>
        : ServerBase
        where TExceptionHandler : class
    {
        protected Socket ListenerSocket;
        protected readonly THandler HandlerFunc;
        protected readonly TExceptionHandler ExceptionHandlerFunc;

        public IPEndPoint LocalEndPoint { get; protected set; }

        protected SocketServerBase(THandler handlerFunc, IPAddress ip, int port, TExceptionHandler exceptionHandlerFunc)
        {
            HandlerFunc = handlerFunc;
            LocalEndPoint = new IPEndPoint(ip, port);
            ExceptionHandlerFunc = exceptionHandlerFunc;
        }

        protected SocketServerBase(THandler handlerFunc, IPAddress ip, int port)
            : this(handlerFunc, ip, port, null)
        { }

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
