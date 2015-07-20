using Tesla.ServiceProcess;

namespace Tesla.Supervisor
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
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
