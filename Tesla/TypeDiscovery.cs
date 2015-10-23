using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Tesla {
    public delegate T ObjectActivator<T>(params object[] args);

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

        public static Type TypeFromString(string typeSpec) {
            if (string.IsNullOrWhiteSpace(typeSpec)) {
                return null;
            }

            var typeSpecSplit = typeSpec.Split(',').Select(x => x.Trim()).ToArray();

            if (typeSpecSplit.Length != 2) {
                return null;
            }

            var assembly = Assembly.Load(typeSpecSplit[1]);
            return assembly.GetType(typeSpecSplit[0], false);
        }

        public static ObjectActivator<T> CreateActivator<T>(ConstructorInfo ctorInfo) {
            var type = ctorInfo.DeclaringType;
            var paramsInfo = ctorInfo.GetParameters();
            var paramExpression = Expression.Parameter(typeof(object[]), "args");
            var argsExpressions = new Expression[paramsInfo.Length];

            for (var i = 0; i < paramsInfo.Length; i++) {
                var indexExpression = Expression.Constant(i);
                var paramType = paramsInfo[i].ParameterType;
                var paramAccessorExpression = Expression.ArrayIndex(paramExpression, indexExpression);
                var paramCastExpression = Expression.Convert(paramAccessorExpression, paramType);

                argsExpressions[i] = paramCastExpression;
            }

            var ctorExpression = Expression.New(ctorInfo, argsExpressions);
            var lambdaExpression = Expression.Lambda(typeof(ObjectActivator<T>), ctorExpression, paramExpression);

            return (ObjectActivator<T>)lambdaExpression.Compile();
        }
    }
}
