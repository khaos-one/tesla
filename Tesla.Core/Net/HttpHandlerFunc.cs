using System.Net;
using System.Threading.Tasks;

namespace Tesla.Net
{
    public delegate Task HttpHandlerFunc(HttpListenerContext context);
}
