using System;
using System.Collections.Generic;
using System.Reflection;
using System.ServiceProcess;

namespace Tesla.ServiceProcess
{
    public class ServiceList
        : List<ServiceBase>
    {
        public void Run()
        {
            ServiceBase.Run(ToArray());
        }

        public void RunInteractive()
        {
            Console.WriteLine("Services running in interactive mode.");

            var methodOnStart = typeof (ServiceBase).GetMethod("OnStart", BindingFlags.Instance | BindingFlags.NonPublic);
            ForEach(x =>
            {
                Console.Write("Starting service '{0}'...", x.ServiceName);
                methodOnStart.Invoke(x, new object[] {new string[] {}});
                Console.WriteLine("done.");
            });

            Console.Write("Press <return> key to stop the services...");
            Console.ReadLine();

            var methodOnStop = typeof(ServiceBase).GetMethod("OnStop", BindingFlags.Instance | BindingFlags.NonPublic);
            ForEach(x =>
            {
                Console.Write("Stopping service '{0}'...", x.ServiceName);
                methodOnStop.Invoke(x, null);
                Console.WriteLine("done.");
            });

            Console.WriteLine("All services stopped.");
            Console.Write("Press <return> key to continue...");
            Console.ReadLine();
        }
    }
}
