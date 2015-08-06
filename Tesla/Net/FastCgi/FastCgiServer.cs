using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Tesla.Net.FastCgi
{
    public class FastCgiServer
        : ThreadedTcpServer
    {
        protected const int MaxResponseSize = 30000;

        public FastCgiServer(IPAddress ip, int port)
            : base(ip, port)
        { }

        public FastCgiServer(int port)
            : base(port)
        { }

        protected override void HandleException(Socket socket, Exception e)
        {
            throw new NotImplementedException();
        }

        protected override void HandleRequest(Socket socket)
        {
            throw new NotImplementedException();
        }
    }
}
