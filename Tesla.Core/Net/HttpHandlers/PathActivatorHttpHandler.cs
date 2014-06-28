using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Tesla.Net.HttpHandlers
{
    public class PathActivatorHttpHandler
        : HttpHandlerBase,
          IDictionary<string, IHttpHandler>
    {
        protected readonly IDictionary<string, IHttpHandler> Bindings =
            new Dictionary<string, IHttpHandler>();

        public void AddBinding(string path, IHttpHandler handler)
        {
            Bindings.Add(path, handler);
        }

        public override async Task Handle(HttpListenerContext context)
        {
            var path = context.Request.Url.AbsolutePath;

            if (Bindings.ContainsKey(path))
                await Bindings[path].Handle(context);
            else
                throw new HttpException(HttpStatusCode.NotFound);
        }

        public void Add(string key, IHttpHandler value)
        {
            Bindings.Add(key, value);
        }

        public bool ContainsKey(string key)
        {
            return Bindings.ContainsKey(key);
        }

        public ICollection<string> Keys
        {
            get { return Bindings.Keys; }
        }

        public bool Remove(string key)
        {
            return Bindings.Remove(key);
        }

        public bool TryGetValue(string key, out IHttpHandler value)
        {
            return Bindings.TryGetValue(key, out value);
        }

        public ICollection<IHttpHandler> Values
        {
            get { return Bindings.Values; }
        }

        public IHttpHandler this[string key]
        {
            get { return Bindings[key]; }
            set { Bindings[key] = value; }
        }

        public void Add(KeyValuePair<string, IHttpHandler> item)
        {
            Bindings.Add(item);
        }

        public void Clear()
        {
            Bindings.Clear();
        }

        public bool Contains(KeyValuePair<string, IHttpHandler> item)
        {
            return Bindings.Contains(item);
        }

        public void CopyTo(KeyValuePair<string, IHttpHandler>[] array, int arrayIndex)
        {
            Bindings.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return Bindings.Count; }
        }

        public bool IsReadOnly
        {
            get { return Bindings.IsReadOnly; }
        }

        public bool Remove(KeyValuePair<string, IHttpHandler> item)
        {
            return Bindings.Remove(item);
        }

        public IEnumerator<KeyValuePair<string, IHttpHandler>> GetEnumerator()
        {
            return Bindings.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return Bindings.GetEnumerator();
        }
    }
}
