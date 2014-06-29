using System.Net;

namespace Tesla.Net
{
    public static class IPEndPointExtensions
    {
        public static IPEndPoint Any = new IPEndPoint(IPAddress.Any, 0);
        public static IPEndPoint IPv6Any = new IPEndPoint(IPAddress.IPv6Any, 0);
    }
}
