using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Tesla.Extensions;

namespace Tesla.Serialization {
    public sealed class Csv
        : DataTable<string> {
        private static readonly Regex CsvLineRegex =
            new Regex(@"(((?<x>(?=[,\r\n]+))|""(?<x>([^""]|"""")+)""|(?<x>[^,\r\n]+)),?)",
                RegexOptions.Compiled | RegexOptions.ExplicitCapture);

        public Csv() {}

        public Csv(string content) {
            unchecked {
                var lines = content.Split('\n');

                if (lines.Length == 0) {
                    HasData = false;
                    return;
                }

                var header = SplitLine(lines[0]);
                ColumnsCount = header.Length;

                for (var i = 0; i < ColumnsCount; i++) {
                    _columns.Add(header[i], i);
                }

                if (lines.Length < 2) {
                    HasData = false;
                }
                else {
                    for (var i = 1; i < lines.Length; i++) {
                        if (string.IsNullOrEmpty(lines[i])) {
                            continue;
                        }

                        _data.Add(SplitLine(lines[i]));
                    }
                }
            }
        }

        private string[] SplitLine(string line) {
            var matches = CsvLineRegex.Matches(line.Trim('\r', ' ', '\t'));
            var list = new List<string>(matches.Count);


            foreach (Match match in matches) {
                list.Add(match.Groups[1].Value);
            }

            if (_columns.Count > 0) {
                var diff = _columns.Count - matches.Count;

                if (diff > 0) {
                    list.AddRange(string.Empty.MultiplyReference(diff));
                }
                else if (diff < 0) {
                    list.RemoveRange(list.Count + diff, Math.Abs(diff));
                }
            }

            return list.ToArray();
        }
    }
}
