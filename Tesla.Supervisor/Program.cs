using Tesla.ServiceProcess;

namespace Tesla.Supervisor
{
    static class Program
    {
        /// <summary>
        /// Main application entry point.
        /// </summary>
        static void Main()
        {
            var list = new ServiceList
            {
                new SupervisorService()
            };
            list.RunInteractive();
        }
    }
}
