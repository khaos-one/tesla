using System.Collections.Generic;

namespace Tesla.Protocol.Types
{
    public class WriteProtectedList<T>
        : List<T>
    {
        public WriteProtectedList()
            : base()
        { }

        public WriteProtectedList(int capacity)
            : base(capacity)
        { }

        public WriteProtectedList(IEnumerable<T> collection)
            : base(collection)
        { }

        public new T this[int index]
        {
            get { return base[index]; }
            protected set { base[index] = value; }
        }

        protected new void Insert(int index, T item)
        {
            base.Insert(index, item);
        }

        protected new void RemoveAt(int index)
        {
            base.RemoveAt(index);
        }
    }
}
