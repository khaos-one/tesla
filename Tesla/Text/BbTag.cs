using System;
using System.Text.RegularExpressions;

namespace Tesla.Text {
    public sealed class BbTag {
        private string OverallReplacement;
        private string OverallSearch;
        private Regex OpeningRegex;
        private Regex OverallRegex;

        public BbTag(string openingPattern, string middlePattern, string closingPattern, string openingReplacement, string middleReplacement, string closingReplacement) {
            OverallReplacement = openingReplacement + middleReplacement + closingReplacement;

            OpeningRegex = new Regex(openingPattern,
                RegexOptions.IgnoreCase |
                RegexOptions.Singleline |
                RegexOptions.Compiled);
            OverallRegex = new Regex(openingPattern + middlePattern + closingPattern, 
                RegexOptions.IgnoreCase | 
                RegexOptions.Singleline | 
                RegexOptions.Compiled);
        }

        public BbTag(string openingPattern, string closingPattern, string openingReplacement, string closingReplacement) {
            OverallReplacement = openingReplacement + closingReplacement;

            OpeningRegex = new Regex(openingPattern,
                RegexOptions.IgnoreCase |
                RegexOptions.Singleline |
                RegexOptions.Compiled);
            //OverallRegex = new Regex(openingPattern + middlePattern + closingPattern,
            //    RegexOptions.IgnoreCase |
            //    RegexOptions.Singleline |
            //    RegexOptions.Compiled);
        }

        public BbTag(string pattern, string replacement, bool regex = true) {
            OverallReplacement = replacement;

            if (regex) {
                OverallRegex = new Regex(pattern,
                    RegexOptions.IgnoreCase |
                    RegexOptions.Singleline |
                    RegexOptions.Compiled);
            } else {
                OverallSearch = pattern;
            }
        }

        public string Transform(string text) {
            if (OverallRegex != null) {
                text = OverallRegex.Replace(text, OverallReplacement);
            } else if (OverallSearch != null) {
                text = text.Replace(OverallSearch, OverallReplacement);
            } else {
                throw new ArgumentException("No search pattern defined.");
            }

            if (OpeningRegex != null) {
                text = OpeningRegex.Replace(text, string.Empty);
            }
            // TODO: Check for unmatched opening tags and close them at the end.

            return text;
        }
    }
}
