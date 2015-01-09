using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Tesla.IO;

namespace Tesla.Net.HttpHandlers
{
    [Obsolete]
    public class StaticServeHttpHandler
        : HttpHandlerBase
    {
        internal static string[] FileExtensions =
        {
            ".html", ".htm", ".css", ".xml", ".gif", ".jpeg", ".jpg", ".svg", 
            ".js", ".txt", ".png", ".ico", ".pdf",
            ".zip", ".woff", ".ttf"
        };
        internal static string[] MimeTypes =
        {
            "text/html", "text/html", "text/css", "text/xml", "image/gif", "image/jpeg", "image/jpeg", "image/svg+xml",
            "application/x-javascript", "text/plain", "image/png", "image/x-icon", "application/pdf",
            "application/zip", "application/x-woff", "application/x-font-ttf"
        };

        protected string BasePath;

        public StaticServeHttpHandler(string basePath)
        {
            BasePath = basePath;
        }

        public override async Task Handle(HttpListenerContext context)
        {
            var path = context.Request.Url.AbsolutePath;

            if (Path.HasExtension(path))
            {
                var idx = Array.IndexOf(FileExtensions, Path.GetExtension(path));

                if (idx == -1)
                {
                    if (NextHandler != null)
                        await NextHandler.Handle(context);
                    else
                        throw new HttpException(HttpStatusCode.NotFound);
                }
                else
                {
                    var absolutePath = Path.Combine(BasePath, path.TrimStart('/', '\\'));

                    if (absolutePath.IndexOf(BasePath, StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        if (File.Exists(absolutePath))
                        {
                            context.Response.ContentType = MimeTypes[idx];
                            await context.Response.OutputStream.WriteFileAsync(absolutePath);

                            //using (var fs = new FileStream(absolutePath, FileMode.Open, FileAccess.Read))
                            //{
                            //    await fs.CopyToAsync(context.Response.OutputStream);
                            //}
                        }
                        else
                        {
                            throw new HttpException(HttpStatusCode.NotFound);
                        }
                    }
                    else
                    {
                        throw new HttpException(HttpStatusCode.Forbidden);
                    }
                }
            }
            else
            {
                if (NextHandler != null)
                    await NextHandler.Handle(context);
                else
                    throw new HttpException(HttpStatusCode.NotFound);
            }
        }
    }
}
