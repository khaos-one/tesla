using System;
using YamlDotNet.Serialization;

namespace Tesla.Supervisor {
    [Serializable]
    public sealed class ApplicationConfig {
        [YamlMember(Alias = "executable")]
        public string Executable { get; set; }

        [YamlMember(Alias = "args")]
        public string Arguments { get; set; }

        [YamlMember(Alias = "working-dir")]
        public string WorkingDirectory { get; set; }

        [YamlMember(Alias = "exec-as-user")]
        public string ExecuteAsUser { get; set; }

        [YamlMember(Alias = "out-file")]
        public string OutFile { get; set; }

        [YamlMember(Alias = "error-file")]
        public string ErrorFile { get; set; }

        [YamlMember(Alias = "autorestart")]
        public bool Autorestart { get; set; }
    }
}
