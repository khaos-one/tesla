using System;
using YamlDotNet.Serialization;

namespace Tesla.Supervisor {
    [Serializable]
    public sealed class ServerConfig {
        [YamlMember(Alias = "address")]
        public string Address { get; set; }

        [YamlMember(Alias = "port")]
        public ushort Port { get; set; }
    }
}
