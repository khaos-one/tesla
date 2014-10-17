using System;
using System.Net.Sockets;

namespace Tesla.Net
{
    public static class TcpClientExtensions
    {
        public static void ConnectWithTimeout(this TcpClient client, string hostname, int port, int timeout = 5000)
        {
            var t = client.ConnectAsync(hostname, port);

            try
            {
                t.Wait(timeout);
            }
            catch (AggregateException e)
            {
                throw e.InnerException;
            }

            if (!t.IsCompleted)
            {
                throw new TimeoutException("Connection timeout.");
            }
        }
    }
}
