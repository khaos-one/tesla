using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using Tesla.Logging;

namespace Tesla.Net.FastCgi
{
    public class FastCgiServer
        : ThreadedTcpServer
    {
        public FastCgiServer(IPAddress ip, int port)
            : base(ip, port)
        { }

        public FastCgiServer(int port)
            : base(port)
        { }

        protected override void HandleException(Socket socket, Exception e)
        {
            Log.Entry(Priority.Warning, "FastCgiServer: Unhandler exception: {0}", e);
        }

        protected override void HandleRequest(Socket socket)
        {
            FastCgiRecord record = null;
            FastCgiBeginRequest requestInfo = null;
            FastCgiParams parameters = new FastCgiParams();
            MemoryStream input = null;
            bool responseSent = false;

            using (var stream = socket.GetStream())
            {
                while (true)
                {
                    if (!FastCgiRecord.TryParse(stream, out record))
                        return;

                    switch (record.Type)
                    {
                        case FastCgiRecord.RecordType.BeginRequest:
                            // Request transmission start
                            requestInfo = new FastCgiBeginRequest(record);
                            parameters.Clear();
                            CloseStream(input);
                            break;

                        case FastCgiRecord.RecordType.AbortRequest:
                            // Abort the request
                            // The connection should be preserved if FCGI_KEEP_CONN flag
                            // was set, other BEGIN_REQUEST record should arrive next
                            if (!responseSent)
                            {
                                responseSent = true;
                                var endRequest = record.GetEndRequestPacket();
                                var closeSocket = (requestInfo.Flags & 0x1) > 0;

                                // TODO: Make use of async 
                                //socket.BeginSend(endRequest, 0, endRequest.Length, SocketFlags.None, Socket_DataSent, new Tuple<Socket, bool>(socket, closeSocket));
                                stream.Write(endRequest, 0, endRequest.Length);

                                if (closeSocket)
                                    return;
                            }
                            break;

                        case FastCgiRecord.RecordType.EndRequest:
                            // This record should only be sent from an app
                            // to webserver
                            throw new InvalidOperationException();

                        case FastCgiRecord.RecordType.Params:
                            if (!parameters.EndReached)
                                parameters.AddRecord(record);
                            break;

                        case FastCgiRecord.RecordType.Data:
                            // This record type is not used for responder apps
                            throw new InvalidOperationException();

                        case FastCgiRecord.RecordType.StdIn:
                            if (record.ContentLength > 0)
                            {
                                if (input == null)
                                    input = new MemoryStream();

                                input.Write(record.ContentData, 0, record.ContentLength);
                            }
                            else
                            {
                                // Zero-length STDIN received that means
                                // that all the data had been sent and 
                                // request should be executed
                                // Actual processing here.

                                // Check lengthes
                                int claimedLength = 0;

                                if (parameters.ContainsKey("CONTENT_LENGTH"))
                                {
                                    int.TryParse(parameters["CONTENT_LENGTH"], out claimedLength);
                                }

                                if (input != null && input.Length < claimedLength)
                                {
                                    // End the request
                                    if (!responseSent)
                                    {
                                        responseSent = true;
                                        var endRequest = record.GetEndRequestPacket();
                                        var closeSocket = (requestInfo.Flags & 0x1) > 0;

                                        // TODO: Make use of async 
                                        //socket.BeginSend(endRequest, 0, endRequest.Length, SocketFlags.None, Socket_DataSent, new Tuple<Socket, bool>(socket, closeSocket));
                                        stream.Write(endRequest, 0, endRequest.Length);

                                        if (closeSocket)
                                            return;
                                    }
                                }
                                else
                                {
                                    string result = string.Empty;

                                    try
                                    {
                                        if (input != null)
                                        {
                                            input.Position = 0;
                                            input.SetLength(claimedLength);
                                        }

                                        result = "Content-Type: text/html; charset=utf8\n\n<html><body>It fuckking works</body></html>";
                                    }
                                    catch (Exception e)
                                    {
                                        Log.Entry(Priority.Warning, "FastCgiServer: exception thrown during processing: {0}", e);
                                    }

                                    // Send the result back.
                                    responseSent = true;
                                    var endRequest = record.GetResultPackets(result);
                                    var closeSocket = (requestInfo.Flags & 0x1) > 0;

                                    // TODO: Make use of async 
                                    //socket.BeginSend(endRequest, 0, endRequest.Length, SocketFlags.None, Socket_DataSent, new Tuple<Socket, bool>(socket, closeSocket));
                                    stream.Write(endRequest, 0, endRequest.Length);

                                    if (closeSocket)
                                        return;
                                }
                            }
                            break;

                        case FastCgiRecord.RecordType.GetValues:
                            // Not yet implemented
                            throw new NotImplementedException();

                        default:
                            break;
                    }
                }
            }
        }

        private static void CloseStream(Stream s)
        {
            if (s != null)
            {
                s.Close();
                s.Dispose();
            }
        }

        private static void Socket_DataSent(IAsyncResult ar)
        {
            var state = ar as Tuple<Socket, bool>;

            try
            {
                state.Item1.EndSend(ar);

                if (state.Item2)
                {
                    state.Item1.Close(10);
                }
            }
            catch
            {
                try
                {
                    state.Item1.Close();
                }
                catch
                { }
            }
        }
    }
}
