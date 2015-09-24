using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.ServiceProcess;
using Tesla.Logging;
using Tesla.Supervisor.Configuration;

namespace Tesla.Supervisor {
    public partial class SupervisorService : ServiceBase {
        public List<Application> Applications = new List<Application>();
        public SupervisorConfig Configuration;
        public Stream LogStream;

        public SupervisorService() {
            InitializeComponent();
        }

        protected override void OnStart(string[] args) {
            var configPath =
                ConfigurationManager.AppSettings["config"] ?? "Default.json";

            if (!File.Exists(configPath))
                throw new Exception("Cannot find any configuration file.");

            using (var fs = File.OpenText(configPath)) {
                var serializer = new JsonSerializer();
                Configuration = (SupervisorConfig) serializer.Deserialize(fs, typeof (SupervisorConfig));
            }

            LogStream = !string.IsNullOrEmpty(Configuration.LogFile)
                ? File.Open(Configuration.LogFile, FileMode.Append)
                : File.Create("Supervisor.log");
            Log.DefaultLogStream = LogStream;

            Configuration.Apps.ForEach(x => Applications.Add(new Application(x)));
            Applications.ForEach(x => x.Start());

            Log.Entry(Priority.Info, "Supervisor applications started.");
        }

        protected override void OnStop() {
            Applications.ForEach(x => x.Stop());
            Applications.Clear();
        }
    }
}