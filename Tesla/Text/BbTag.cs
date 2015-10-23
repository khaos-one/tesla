using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Tesla.Text {
    public sealed class BbTag {
        public string OpeningPattern;
        public string ClosingPattern;
        public string OpeningReplacement;
        public string ClosingReplacement;

        private Regex OpeningRegex;
        private Regex ClosingRegex;

        public BbTag(string openingPattern, string closingPattern, string openingReplacement, string closingReplacement) {
            OpeningPattern = openingPattern;
            ClosingPattern = closingPattern;
            OpeningReplacement = openingReplacement;
            ClosingReplacement = closingReplacement;

            OpeningRegex = new Regex(OpeningPattern,
                RegexOptions.IgnoreCase |
                RegexOptions.Singleline |
                RegexOptions.Compiled);
            ClosingRegex = new Regex(ClosingPattern, 
                RegexOptions.IgnoreCase | 
                RegexOptions.Singleline | 
                RegexOptions.Compiled);
        }

        public string Transform(string text) {
            text = OpeningRegex.Replace(text, OpeningReplacement);
            text = ClosingRegex.Replace(text, ClosingReplacement);

            // TODO: Check for unmatched opening tags and close them at the end.
            return text;
        }
    }
}
