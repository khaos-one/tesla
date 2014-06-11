using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Tesla.Net
{
    public static class SocketExtensions
    {
        internal const int MaxUdpDatagramSize = 0x10000;

        public static Task ConnectAsync(this Socket socket, EndPoint endPoint)
        {
            return Task.Factory.FromAsync(socket.BeginConnect, socket.EndConnect, endPoint, null);
        }

        public static Task<Socket> AcceptAsync(this Socket socket)
        {
            return Task<Socket>.Factory.FromAsync(socket.BeginAccept, socket.EndAccept, null);
        }

        //public static Task<int> ReceiveFromAsync(this Socket socket, byte[] buffer, int offset, int size, SocketFlags flags, ref EndPoint endPoint)
        //{
        //    UdpClient
        //    return Task<int>.Factory.FromAsync(socket.BeginReceiveFrom(buffer, offset, size, flags, ref endPoint), socket.EndReceiveFrom(null, ref endPoint), null);
        //}

        public static IAsyncResult BeginReceiveFrom(this Socket socket, AsyncCallback requestCallback)
        {
            EndPoint endPoint;

            switch (socket.AddressFamily)
            {
                case AddressFamily.InterNetwork:
                    endPoint = IPEndPointExtensions.Any;
                    break;
                default:
                    endPoint = IPEndPointExtensions.IPv6Any;
                    break;
            }

            var buffer = BufferManagerExtensions.Instance().TakeBuffer(MaxUdpDatagramSize);
            return socket.BeginReceiveFrom(buffer, 0, buffer.Length, SocketFlags.None, ref endPoint, requestCallback, buffer);
        }

        public static byte[] EndReceiveFrom(this Socket socket, IAsyncResult asyncResult, ref IPEndPoint remoteEndPoint)
        {
            EndPoint endPoint;

            switch (socket.AddressFamily)
            {
                case AddressFamily.InterNetwork:
                    endPoint = IPEndPointExtensions.Any;
                    break;
                default:
                    endPoint = IPEndPointExtensions.IPv6Any;
                    break;
            }
            
            var received = socket.EndReceiveFrom(asyncResult, ref endPoint);
            remoteEndPoint = (IPEndPoint) endPoint;

            var buffer = (byte[]) asyncResult.AsyncState;
            var returnBuffer = new byte[received];

            Buffer.BlockCopy(buffer, 0, returnBuffer, 0, returnBuffer.Length);
            BufferManagerExtensions.Instance().ReturnBuffer(buffer);

            return returnBuffer;
        }

        public static Task<Tuple<byte[], IPEndPoint>> ReceiveAsync(this Socket socket)
        {
            return Task<Tuple<byte[], IPEndPoint>>.Factory.FromAsync(
                (callback, state) => socket.BeginReceiveFrom(callback),
                (ar) =>
                {
                    IPEndPoint remoteIPEndPoint = null;
                    var received = socket.EndReceiveFrom(ar, ref remoteIPEndPoint);
                    return new Tuple<byte[], IPEndPoint>(received, remoteIPEndPoint);
                }, null);
        }

        public static NetworkStream GetStream(this Socket socket)
        {
            return new NetworkStream(socket);
        }
    }
}
