using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading;
using Tesla.Logging;
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
        private readonly uint _startupTime;
        private readonly uint _interval;
        private StreamWriter _stdErr;
        private StreamWriter _stdOut;
        private byte _hangCount;
        private readonly string _webCheckRequestUri;
        private readonly uint _recheckCount;
        private readonly uint _webCheckRequestTimeout;
        private bool _plannedStop;
        private readonly bool _hangDetection;

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

            if (!string.IsNullOrEmpty(app.ErrorFile))
            {
                _stdErr = !File.Exists(app.ErrorFile)
                    ? File.CreateText(app.ErrorFile)
                    : new StreamWriter(app.ErrorFile, true);
                _startInfo.RedirectStandardError = true;
            }

            if (!string.IsNullOrEmpty(app.OutputFile))
            {
                _stdErr = !File.Exists(app.OutputFile)
                    ? File.CreateText(app.OutputFile)
                    : new StreamWriter(app.OutputFile, true);
                _startInfo.RedirectStandardOutput = true;
            }

            if (app.UseHangDetection)
            {
                _webCheckRequestUri = app.WebCheckRequestUri;
                _webCheckRequestTimeout = app.WebCheckRequestTimeout;
                _startupTime = app.StartupTime;
                _interval = app.RecheckInterval;
                _recheckCount = app.RecheckCount;
            }

            _hangDetection = app.UseHangDetection;
        }

        private void TimerCallback(object state)
        {
            if (_stopping)
                return;

            var request = WebRequest.Create(_webCheckRequestUri);
            request.Timeout = (int) _webCheckRequestTimeout;

            try
            {
                var response = (HttpWebResponse) request.GetResponse();

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    Log.Entry(Priority.Warning, "Web service `{0}` responded with error status `{1} - {2}`.",
                        _startInfo.FileName, (int) response.StatusCode, response.StatusDescription);
                }
            }
            catch (WebException)
            {
                _hangCount++;

                if (_hangCount >= _recheckCount)
                {
                    Restart();
                }
            }
        }

        private void Process_Exited(object sender, EventArgs e)
        {
            if (_stopping)
                return;

            if (!_plannedStop)
                Log.Entry(Priority.Warning, "Application `{0}` unexpectedly exited with code {1}, restarting.",
                    _startInfo.FileName, _process.ExitCode);

            Start();
        }

        private void Process_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (_stopping)
                return;

            if (!string.IsNullOrEmpty(e.Data))
                _stdErr.WriteLine(e.Data);
        }

        private void Process_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (_stopping)
                return;

            if (!string.IsNullOrEmpty(e.Data))
                _stdErr.WriteLine(e.Data);
        }

        public void Start()
        {
            _stopping = false;
            _process = new Process { StartInfo = _startInfo, EnableRaisingEvents = true };
            _process.Exited += Process_Exited;

            if (_stdErr != null)
                _process.ErrorDataReceived += Process_ErrorDataReceived;

            if (_stdOut != null)
                _process.OutputDataReceived += Process_OutputDataReceived;

            _process.Start();

            if (_stdErr != null)
                _process.BeginErrorReadLine();

            if (_stdOut != null)
                _process.BeginOutputReadLine();

            if (_hangDetection)
                _timer = new Timer(TimerCallback, null, _startupTime, _interval);
        }

        public void Stop()
        {
            _stopping = true;

            if (_timer != null)
                _timer.Change(Timeout.Infinite, Timeout.Infinite);

            _plannedStop = true;

            if (_stdErr != null)
                _process.CancelErrorRead();

            if (_stdOut != null)
                _process.CancelOutputRead();

            if (!_process.HasExited)
            {
                _process.Kill();
                _process.WaitForExit(500);
                _plannedStop = false;
            }

            _process = null;

            if (_stdErr != null)
            {
                _stdErr.Close();
                _stdErr.Dispose();
                _stdErr = null;
            }

            if (_stdOut != null)
            {
                _stdOut.Close();
                _stdOut.Dispose();
                _stdOut = null;
            }
        }

        public void Restart()
        {
            Stop();
            _hangCount = 0;
            Start();
        }

        public void Dispose()
        {
            Stop();
        }
    }
}
