using System.Collections.Generic;

namespace Tesla.Supervisor.Configuration {
    public sealed class SupervisorAppConfig {
        public string Executable { get; set; }
        public string Arguments { get; set; }
        public string User { get; set; }
        public string WorkingDirectory { get; set; }
        public uint StartupTime { get; set; }
        public string ErrorFile { get; set; }
        public string OutputFile { get; set; }
        public bool UseHangDetection { get; set; }
        public uint RecheckInterval { get; set; }
        public uint RecheckCount { get; set; }
        public string WebCheckRequestUri { get; set; }
        public uint WebCheckRequestTimeout { get; set; }
    }

    public sealed class SupervisorConfig {
        public List<SupervisorAppConfig> Apps;

        public SupervisorConfig() {
            Apps = new List<SupervisorAppConfig>();
        }

        public string LogFile { get; set; }
    }
}