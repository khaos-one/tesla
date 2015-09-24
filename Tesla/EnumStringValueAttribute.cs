using System;

namespace Tesla {
    [Obsolete]
    [AttributeUsage(AttributeTargets.Field)]
    public sealed class EnumStringValueAttribute
        : Attribute {
        public EnumStringValueAttribute(string value) {
            Value = value;
        }

        public string Value { get; }

        private static EnumStringValueAttribute[] GetAttributes(Enum val) {
            var type = val.GetType();
            var fi = type.GetField(val.ToString());
            return fi.GetCustomAttributes(typeof (EnumStringValueAttribute), false) as EnumStringValueAttribute[];
        }

        public static string GetValue(Enum val) {
            var attrs = GetAttributes(val);

            if (attrs.Length > 0)
                return attrs[0].Value;
            return null;
        }

        public static string[] GetValues(Enum val) {
            var attrs = GetAttributes(val);

            if (attrs.Length > 0) {
                var result = new string[attrs.Length];

                for (var i = 0; i < attrs.Length; i++)
                    result[i] = attrs[i].Value;

                return result;
            }
            return null;
        }

        public static string GetValues(Enum val, char separator) {
            var attrs = GetAttributes(val);

            if (attrs.Length > 0) {
                var result = string.Empty;

                for (var i = 0; i < attrs.Length; i++)
                    result += attrs[i].Value + separator;

                return result.Trim(' ', separator);
            }
            return null;
        }
    }
}