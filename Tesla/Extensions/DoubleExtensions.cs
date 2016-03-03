using System.Collections.Generic;
using System.Linq;

namespace Tesla.Extensions {
    public static class DoubleExtensions {
        public static double Mean(this IList<double> data) {
            return data.Sum()/data.Count;
        }
    }
}