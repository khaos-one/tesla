using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Tesla
{
    public static class RegexExtensions
    {
        public static string[] GetMatchingGroups(this Regex regex, string str)
        {
            var m = regex.Match(str);
            var ret = new string[m.Groups.Count];

            for (var i = 0; i < m.Groups.Count; i++)
                ret[i] = m.Groups[i].Value;

            return ret;
        }
    }
}
