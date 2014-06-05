using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Tesla.Net
{
    public static class SocketExtensions
    {
        public static Task ConnectAsync(this Socket socket, EndPoint endPoint)
        {
            return Task.Factory.FromAsync(socket.BeginConnect, socket.EndConnect, endPoint, null);
        }

        public static Task<Socket> AcceptAsync(this Socket socket)
        {
            return Task<Socket>.Factory.FromAsync(socket.BeginAccept, socket.EndAccept, null);
        }

        public static NetworkStream GetStream(this Socket socket)
        {
            return new NetworkStream(socket);
        }
    }
}
