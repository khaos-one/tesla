using System.Net;
using System.Threading.Tasks;

namespace Tesla.Net
{
    public interface IHttpHandler
    {
        Task Handle(HttpListenerContext context);
    }
}
