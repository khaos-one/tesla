using System;
using System.Collections.Generic;

namespace SlashCube.Server
{
    public class TypeDispatcher<TIn, TOut>
        : Dictionary<Type, Func<TIn, TOut>>
    {
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

            return default(TOut);
        }
    }
}
