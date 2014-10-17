using System;
using System.Collections.Generic;
using System.Linq;

namespace Tesla.Protocol
{
    public static class TypeDiscovery
    {
        private static readonly IDictionary<Type, IList<Type>> AttributesMapDictionary = new Dictionary<Type, IList<Type>>();
        private static readonly IDictionary<Type, IList<Type>> SubclassesDictionary = new Dictionary<Type, IList<Type>>(); 

        public static IList<Type> FindTypesWithAttribute<TAttribute>()
            where TAttribute : Attribute
        {
            var type = typeof (TAttribute);

            if (AttributesMapDictionary.ContainsKey(type)) 
                return AttributesMapDictionary[type];

            var types = (from a in AppDomain.CurrentDomain.GetAssemblies()
                from t in a.GetTypes()
                where t.IsDefined(t, false)
                select t).ToList();
            AttributesMapDictionary.Add(type, types);

            return types;
        }

        public static IList<Type> FindSubclassTypes<T>()
            where T : class 
        {
            var type = typeof (T);

            //if (SubclassesDictionary.ContainsKey(type))
            //    return SubclassesDictionary[type] as IList<T>;

            var types = (from a in AppDomain.CurrentDomain.GetAssemblies()
                from t in a.GetTypes()
                where t.IsSubclassOf(type)
                select t).ToList();
            //SubclassesDictionary.Add(type, types);

            return types;
        }
    }
}
