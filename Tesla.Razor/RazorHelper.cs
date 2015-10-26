using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RazorEngine;
using RazorEngine.Templating;

namespace Tesla.Razor {
    public static class RazorHelper {
        public static void PrecompileTemplates(string templatesDirectory, bool excludeExtensions = false) {
            var templateFiles = Directory.GetFiles(templatesDirectory, "*.cshtml", SearchOption.AllDirectories);

            foreach (var file in templateFiles) {
                var contents = File.ReadAllText(file);
                var templateName = file.Replace(templatesDirectory + Path.DirectorySeparatorChar, string.Empty).Replace(Path.DirectorySeparatorChar, '/');

                if (excludeExtensions) {
                    templateName = templateName.Replace(".cshtml", string.Empty);
                }

                Engine.Razor.Compile(contents, templateName);
            }
        }
    }
}
