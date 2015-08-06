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
            using (var stream = socket.GetStream())
            {
                FastCgiRecord record;

                if (FastCgiRecord.TryParse(stream, out record))
                    return;

                switch (record.Type)
                {
                    case FastCgiRecord.RecordType.BeginRequest:
                        throw new NotImplementedException();
                        break;

                    case FastCgiRecord.RecordType.AbortRequest:
                        throw new NotImplementedException();
                        break;

                    case FastCgiRecord.RecordType.EndRequest:
                        // This record should only be sent from an app
                        // to webserver
                        throw new InvalidOperationException();

                    case FastCgiRecord.RecordType.Params:
                        throw new NotImplementedException();
                        break;

                    case FastCgiRecord.RecordType.Data:
                        // This record type is not used for responder apps
                        throw new InvalidOperationException();
                        break;

                    case FastCgiRecord.RecordType.StdIn:
                        throw new NotImplementedException();
                        break;

                    case FastCgiRecord.RecordType.GetValues:
                        throw new NotImplementedException();
                        break;

                    default:
                        break;
                }
            }
        }
    }
}
