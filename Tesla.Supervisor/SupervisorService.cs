using System.ServiceProcess;

namespace Tesla.Supervisor
{
    public partial class SupervisorService : ServiceBase
    {
        public SupervisorService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
        }

        protected override void OnStop()
        {
        }
    }
}
