using System.Collections.Generic;
namespace Tesla.Supervisor.Configuration
{
    public sealed class SupervisorAppConfig
    {
        public string Executable { get; set; }
        public string Arguments { get; set; }
        public string User { get; set; }
        public string WorkingDirectory { get; set; }
        public uint StartupTime { get; set; }
        public uint RecheckInterval { get; set; }
    }

    public sealed class SupervisorConfig
    {
        public List<SupervisorAppConfig> Apps;

        public SupervisorConfig()
        {
            Apps = new List<SupervisorAppConfig>();
        }
    }
}
