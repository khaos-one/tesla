using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.ServiceProcess;
using Newtonsoft.Json;
using Tesla.Supervisor.Configuration;

namespace Tesla.Supervisor
{
    public partial class SupervisorService : ServiceBase
    {
        public SupervisorConfig Configuration;
        public List<Application> Applications = new List<Application>();

        public SupervisorService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            var configPath = 
                ConfigurationManager.AppSettings["config"] ?? "Default.json";

            if (!File.Exists(configPath))
                throw new Exception("Cannot find any configuration file.");

            using (var fs = File.OpenText(configPath))
            {
                var serializer = new JsonSerializer();
                Configuration = (SupervisorConfig) serializer.Deserialize(fs, typeof(SupervisorConfig));
            }

            Configuration.Apps.ForEach(x => Applications.Add(new Application(x)));
            Applications.ForEach(x => x.Start());
        }

        protected override void OnStop()
        {
            Applications.ForEach(x => x.Stop());
            Applications.Clear();
        }
    }
}
