using System.Collections.Generic;
using System.Linq;

namespace Tesla.Collections {
    public static class IEnumerableExtensions {
        public static string JoinString(this IEnumerable<string> collection, string separator = ",") {
            if (collection == null)
                return string.Empty;

            return string.Join(separator,
                collection.Select(i => i.ToString()).Where(s => !string.IsNullOrEmpty(s)).ToArray());
        }
    }
}