using System;
using System.Collections.Generic;

namespace SlashCube.Server
{
    public class TypeDispatcher<TIn, TOut>
        : Dictionary<Type, Func<TIn, TOut>>
    {
        public Func<TIn, TOut> Default { get; protected set; }

        public TypeDispatcher()
        { }

        public TypeDispatcher(Func<TIn, TOut> defaultFunc)
        {
            Default = defaultFunc;
        }

        public TOut Dispatch(object entity)
        {
            var type = entity.GetType();

            foreach (var kv in this)
            {
                if (type == kv.Key)
                {
                    return kv.Value((TIn)entity);
                }
            }

            if (Default == null)
                return default(TOut);

            return Default((TIn)entity);
        }
    }
}
