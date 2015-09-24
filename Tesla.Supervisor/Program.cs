using Tesla.ServiceProcess;

namespace Tesla.Supervisor {
    internal static class Program {
        /// <summary>
        ///     Main application entry point.
        /// </summary>
        private static void Main() {
            var list = new ServiceList {
                new SupervisorService()
            };
            list.RunInteractive();
        }
    }
}