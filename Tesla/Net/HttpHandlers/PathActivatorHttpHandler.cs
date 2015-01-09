using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Tesla.Net.HttpHandlers
{
    [Obsolete]
    public class PathActivatorHttpHandler
        : HttpHandlerBase,
          IDictionary<string, HttpHandlerFunc>
    {
        protected readonly IDictionary<string, HttpHandlerFunc> Bindings =
            new Dictionary<string, HttpHandlerFunc>();

        public override async Task Handle(HttpListenerContext context)
        {
            var path = context.Request.Url.AbsolutePath;

            if (Bindings.ContainsKey(path))
                await Bindings[path](context);
            else
                throw new HttpException(HttpStatusCode.NotFound);
        }

        public void Add(string key, HttpHandlerFunc value)
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

        public bool TryGetValue(string key, out HttpHandlerFunc value)
        {
            return Bindings.TryGetValue(key, out value);
        }

        public ICollection<HttpHandlerFunc> Values
        {
            get { return Bindings.Values; }
        }

        public HttpHandlerFunc this[string key]
        {
            get { return Bindings[key]; }
            set { Bindings[key] = value; }
        }

        public void Add(KeyValuePair<string, HttpHandlerFunc> item)
        {
            Bindings.Add(item);
        }

        public void Clear()
        {
            Bindings.Clear();
        }

        public bool Contains(KeyValuePair<string, HttpHandlerFunc> item)
        {
            return Bindings.Contains(item);
        }

        public void CopyTo(KeyValuePair<string, HttpHandlerFunc>[] array, int arrayIndex)
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

        public bool Remove(KeyValuePair<string, HttpHandlerFunc> item)
        {
            return Bindings.Remove(item);
        }

        public IEnumerator<KeyValuePair<string, HttpHandlerFunc>> GetEnumerator()
        {
            return Bindings.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return Bindings.GetEnumerator();
        }
    }
}
