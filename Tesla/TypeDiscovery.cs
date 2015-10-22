using System;
using System.Linq;
using System.Reflection;

namespace Tesla {
    public static class TypeDiscovery {
        public static T BaseTypeFromString<T>(string objectSpec)
            where T : class {
            if (string.IsNullOrWhiteSpace(objectSpec)) {
                return null;
            }

            var typeSpec = objectSpec.Split(',').Select(x => x.Trim()).ToArray();

            if (typeSpec.Length != 2) {
                return null;
            }

            var assembly = Assembly.Load(typeSpec[1]);
            var type =
                assembly.GetTypes().FirstOrDefault(x => !x.IsAbstract && x.BaseType != null && x.BaseType == typeof (T));

            if (type == null) {
                return null;
            }

            return (T) Activator.CreateInstance(type);
        }

        public static T InterfaceFromString<T>(string objectSpec)
            where T : class {
            if (string.IsNullOrWhiteSpace(objectSpec)) {
                return null;
            }

            var t = typeof (T);

            if (!t.IsInterface) {
                return null;
            }

            var typeSpec = objectSpec.Split(',').Select(x => x.Trim()).ToArray();

            if (typeSpec.Length != 2) {
                return null;
            }

            var assembly = Assembly.Load(typeSpec[1]);
            var type = assembly.GetTypes().FirstOrDefault(x => !x.IsAbstract && x.GetInterface(t.ToString()) != null);

            if (type == null) {
                return null;
            }

            return (T) Activator.CreateInstance(type);
        }
    }
}
