using System.Text.RegularExpressions;

namespace Tesla.Extensions {
    public static class RegexExtensions {
        public static string[] GetMatchingGroups(this Regex regex, string str) {
            var m = regex.Match(str);
            var ret = new string[m.Groups.Count];

            for (var i = 0; i < m.Groups.Count; i++)
                ret[i] = m.Groups[i].Value;

            return ret;
        }
    }
}