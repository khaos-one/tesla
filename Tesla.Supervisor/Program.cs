using System.ServiceProcess;

namespace Tesla.Supervisor
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] 
            { 
                new SupervisorService() 
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
