using System;
using System.Net;
using System.Net.Sockets;
using Tesla.Logging;

namespace Tesla.Net {
    public abstract class ThreadedUdpServer
        : ThreadedServerBase {
        protected Socket ListenerSocket;

        protected ThreadedUdpServer(IPAddress ip, int port) {
            LocalEndPoint = new IPEndPoint(ip, port);
            ListenerSocket = new Socket(AddressFamily.InterNetwork,
                SocketType.Dgram, ProtocolType.Udp);
        }

        protected ThreadedUdpServer(int port)
            : this(IPAddress.Any, port) {}

        public IPEndPoint LocalEndPoint { get; protected set; }

        public int Port => LocalEndPoint.Port;

        public int DatagramSize { get; protected set; }

        protected override void OnStart() {
            try {
                ListenerSocket.Bind(LocalEndPoint);
                ListenerSocket.Listen(500);
                LocalEndPoint = (IPEndPoint) ListenerSocket.LocalEndPoint;
            }
            catch (SocketException) {
                /* TODO: Process exception. */
            }
            catch (ObjectDisposedException) {
                /* TODO: Process exception. */
            }

            Log.Entry(Priority.Info, "[ThreadedUdpServer] [{0}] Server started on port {1}.", ServerName, Port);
        }

        protected override void OnStop() {
            ListenerSocket.Close();
            Log.Entry(Priority.Info, "[ThreadedUdpServer] [{0}] Server stopped on port {1}.", ServerName, Port);
        }

        protected abstract void HandlePacket(Datagram datagram);
        protected abstract void HandleException(Socket socket, Exception e);

        protected override object AcceptClient() {
            var dg = new Datagram(ListenerSocket, DatagramSize);
            var ep = (EndPoint) dg.RemoteEndPoint;

            dg.Read = ListenerSocket.ReceiveFrom(dg.Data, ref ep);

            return dg;
        }

        protected override void HandleClient(object obj) {
            var dg = (Datagram) obj;

            try {
                HandlePacket(dg);
            }
            catch (Exception e) {
                Log.Entry(Priority.Warning, "[ThreadedUdpServer] [{0}] Handler exception: {1}.", ServerName, e);
                HandleException(dg.Socket, e);
            }
        }

        public struct Datagram {
            public Socket Socket;
            public IPEndPoint RemoteEndPoint;
            public byte[] Data;
            public int Read;

            public Datagram(Socket socket, int bufferSize) {
                Socket = socket;
                Data = new byte[bufferSize];
                RemoteEndPoint = null;
                Read = 0;
            }
        }
    }
}