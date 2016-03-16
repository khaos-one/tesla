using System.Collections.Generic;

namespace Tesla.Extensions {
    public static class ObjectExtensions {
        public static IEnumerable<T> MultiplyReference<T>(this T refobj, int count)
            where T : class {
            for (var i = 0; i < count; i++) {
                yield return refobj;
            }
        }

        public static T DefaultOnNull<T>(this T obj, T defaultValue)
            where T : class {
            return obj ?? defaultValue;
        }
    }
}
