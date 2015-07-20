using System;
using System.Diagnostics;
using System.Threading;
using Tesla.Supervisor.Configuration;

namespace Tesla.Supervisor
{
    public sealed class Application
        : IDisposable
    {
        private Process _process;
        private readonly ProcessStartInfo _startInfo;
        private Timer _timer;
        private bool _stopping;
        private uint _startupTime;
        private uint _interval;

        public Application(SupervisorAppConfig app)
        {
            _startInfo = new ProcessStartInfo
            {
                FileName = app.Executable,
                Arguments = app.Arguments,
                UserName = app.User,
                WorkingDirectory = app.WorkingDirectory,
                UseShellExecute = false,
                CreateNoWindow = true
            };
            _startupTime = app.StartupTime;
            _interval = app.RecheckInterval;
        }

        private void TimerCallback(object state)
        {
            if (_stopping)
                return;

            // TODO: Make checks.
            throw new NotImplementedException();
        }

        private void Process_Exited(object sender, EventArgs e)
        {
            if (_stopping)
                return;

            // TODO: Write log entry that process unexpectedly exited.

            Start();
        }

        private void Process_ErrorDataReceived(object sender, EventArgs e)
        {
            if (_stopping)
                return;

            // TODO: Handle error data.
            throw new NotImplementedException();
        }

        private void Process_OutputDatareceived(object sender, EventArgs e)
        {
            if (_stopping)
                return;

            // TODO: Handle output data.
            throw new NotImplementedException();
        }

        public void Start()
        {
            _stopping = false;
            _process = Process.Start(_startInfo);
            _timer = new Timer(TimerCallback, null, _startupTime, _interval);
        }

        public void Stop()
        {
            _stopping = true;
            _timer.Change(Timeout.Infinite, Timeout.Infinite);

            if (!_process.HasExited)
            {
                _process.Close();
                _process.WaitForExit(1000);

                if (!_process.HasExited)
                {
                    _process.Kill();
                }
            }

            _process = null;
        }

        public void Dispose()
        {
            Stop();
        }
    }
}
