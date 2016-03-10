using System;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using Tesla.Logging;

namespace Tesla.Net {
    public abstract class ThreadedSslServer : ThreadedTcpServer {
        public struct SslOptions {
            public X509Certificate ServerCertificate;
            public SslProtocols Protocols;
            public bool RequireClientCertificate;
            public bool CheckRevocation;
        }

        protected SslOptions SslServerOptions;

        public ThreadedSslServer(SslOptions options, IPAddress ip, int port) : base(ip, port) {
            SslServerOptions = options;
        }

        public ThreadedSslServer(SslOptions options, int port) : this(options, IPAddress.Any, port) {}

        protected override void HandleClient(object state) {
            var socket = (Socket) state;

            if (!socket.Connected) {
                Disconnect(socket);
                return;
            }

            try {
                using (var sslStream = new SslStream(socket.GetStream(), false)) {
                    sslStream.AuthenticateAsServer(SslServerOptions.ServerCertificate,
                        SslServerOptions.RequireClientCertificate, SslServerOptions.Protocols,
                        SslServerOptions.CheckRevocation);
                    HandleClient(sslStream);
                }
            }
            finally {
                Disconnect(socket);
            }
        }

        protected override void HandleConnection(Socket socket) {
            throw new NotImplementedException();
        }

        protected override void HandleException(Socket socket, Exception e) {
            throw new NotImplementedException();
        }

        protected abstract void HandleSslClient(SslStream stream);
    }
}
