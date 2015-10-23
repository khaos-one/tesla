using System.Collections.Specialized;
using System.Linq;

namespace Tesla.Collections {
    public static class NameValueCollectionExtensions {
        public static bool HasKey(this NameValueCollection collection, string key) {
            return collection.AllKeys.Contains(key);
        }

        public static bool HasNonEmptyKey(this NameValueCollection collection, string key) {
            if (!collection.HasKeys())
                return false;

            var result = collection.Get(key);
            return !string.IsNullOrWhiteSpace(result);
        }

        public static int? GetInt32(this NameValueCollection collection, string key) {
            var val = collection.Get(key);
            int result;

            if (int.TryParse(val, out result)) {
                return new int?(result);
            }

            return null;
        }
    }
}