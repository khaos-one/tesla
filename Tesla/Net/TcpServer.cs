using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Tesla.Net
{
    using HandlerFunc = Func<Socket, Task>;
    using ErrorFunc = Func<Socket, Exception, bool>;

    public class TcpSocketServer
        : SocketServerBase<HandlerFunc, ErrorFunc>
    {
        public TcpSocketServer(HandlerFunc handlerFunc, IPAddress ip, int port, ErrorFunc errorFunc)
            : base(handlerFunc, ip, port, errorFunc)
        {
            ListenerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //ListenerSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);
            //ListenerSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.DontLinger, true);
        }

        public TcpSocketServer(HandlerFunc handlerFunc, IPAddress ip, int port)
            : this(handlerFunc, ip, port, null)
        { }

        public TcpSocketServer(HandlerFunc handlerFunc, int port) 
            : this(handlerFunc, IPAddress.Any, port)
        { }

        public TcpSocketServer(HandlerFunc handlerFunc) 
            : this(handlerFunc, 0)
        { }

        protected override void BindSocket()
        {
            ListenerSocket.Bind(LocalEndPoint);
            ListenerSocket.Listen(500);

            LocalEndPoint = (IPEndPoint) ListenerSocket.LocalEndPoint;
        }

        protected override async Task<Action> AcceptClient()
        {
            var socket = await ListenerSocket.AcceptAsync();
            return async () =>
            {
                if (!socket.Connected)
                {
                    Disconnect(socket);
                    return;
                }

                try
                {
                    await HandlerFunc(socket);
                }
                catch (Exception e)
                {
                    Trace.TraceWarning("TCP Handler exception: {0}.", e);

                    if (ExceptionHandlerFunc != null)
                    {
                        ExceptionHandlerFunc(socket, e);
                    }
                }
                finally
                {
                    Disconnect(socket);
                }
            };
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
    }
}
