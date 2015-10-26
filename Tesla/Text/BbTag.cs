using System.Text.RegularExpressions;

namespace Tesla.Text {
    public sealed class BbTag {
        public string OpeningPattern;
        public string MiddlePattern;
        public string ClosingPattern;
        public string OpeningReplacement;
        public string MiddleReplacement;
        public string ClosingReplacement;

        private Regex OpeningRegex;
        private Regex OverallRegex;

        public BbTag(string openingPattern, string middlePattern, string closingPattern, string openingReplacement, string middleReplacement, string closingReplacement) {
            OpeningPattern = openingPattern;
            MiddlePattern = middlePattern;
            ClosingPattern = closingPattern;
            OpeningReplacement = openingReplacement;
            MiddleReplacement = middleReplacement;
            ClosingReplacement = closingReplacement;

            OpeningRegex = new Regex(OpeningPattern,
                RegexOptions.IgnoreCase |
                RegexOptions.Singleline |
                RegexOptions.Compiled);
            OverallRegex = new Regex(OpeningPattern + MiddlePattern + ClosingPattern, 
                RegexOptions.IgnoreCase | 
                RegexOptions.Singleline | 
                RegexOptions.Compiled);
        }

        public string Transform(string text) {
            text = OverallRegex.Replace(text, OpeningReplacement + MiddleReplacement + ClosingReplacement);
            text = OpeningReplacement.Replace(text, string.Empty);
            // TODO: Check for unmatched opening tags and close them at the end.

            return text;
        }
    }
}
