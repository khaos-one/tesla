using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tesla.Text {
    public class BbCode {
        private BbTag[] _tags;

        public BbCode(params BbTag[] tags) {
            _tags = tags;
        }

        public string Transform(string text) {
            foreach (var bbtag in _tags) {
                text = bbtag.Transform(text);
            }

            return text;
        }
    }
}
