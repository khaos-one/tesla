using System.Collections.Generic;
using System.Data;

namespace Tesla.Data {
    public static class IDbCommandExtensions {
        public static void Assign(this IDbCommand command, Dictionary<string, object> parameters) {
            if (parameters == null)
                return;

            foreach (var param in parameters) {
                var p = command.CreateParameter();
                p.ParameterName = param.Key;
                p.Value = param.Value;
                command.Parameters.Add(p);
            }
        }
    }
}
