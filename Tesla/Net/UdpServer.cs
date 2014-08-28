using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Tesla.Net
{
    using HandlerFunc = Func<byte[], IPEndPoint, Task>;
    using ErrorFunc = Func<byte[], IPEndPoint, Exception, bool>;

    public sealed class UdpSocketServer
        : SocketServerBase<HandlerFunc, ErrorFunc>
    {
        public UdpSocketServer(HandlerFunc handlerFunc, IPAddress ip, int port)
            : base(handlerFunc, ip, port)
        {
            ListenerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        }

        public UdpSocketServer(HandlerFunc handlerFunc, int port)
            : this(handlerFunc, IPAddress.Any, port)
        { }

        public UdpSocketServer(HandlerFunc handlerFunc)
            : this(handlerFunc, 0)
        { }

        protected override void BindSocket()
        {
            ListenerSocket.Bind(LocalEndPoint);
            LocalEndPoint = (IPEndPoint) ListenerSocket.RemoteEndPoint;
        }

        protected override async Task<Action> AcceptClient()
        {
            var packet = await ListenerSocket.ReceiveAsync();
            return async () =>
            {
                try
                {
                    await HandlerFunc(packet.Item1, packet.Item2);
                }
                catch (Exception e)
                {
                    Trace.TraceWarning("UDP Handler exception: {0}.", e);

                    if (ExceptionHandlerFunc != null)
                    {
                        ExceptionHandlerFunc(packet.Item1, packet.Item2, e);
                    }
                }
            };
        }
    }
}
