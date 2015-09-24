using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading;
using Tesla.Logging;
using Tesla.Supervisor.Configuration;

namespace Tesla.Supervisor {
    /// <summary>
    ///     Represents service application that needs to be maintained online.
    /// </summary>
    public sealed class Application
        : IDisposable {
        /// <summary>Whether to use hang detection.</summary>
        private readonly bool _hangDetection;

        /// <summary>Hang detection check interval.</summary>
        private readonly uint _interval;

        /// <summary>Maximum number of subsequent hangs to restart the process.</summary>
        private readonly uint _recheckCount;

        /// <summary>Process startup information.</summary>
        private readonly ProcessStartInfo _startInfo;

        /// <summary>Hang detection due period (application's warmup).</summary>
        private readonly uint _startupTime;

        /// <summary>Web request timeout to diagnose a hang.</summary>
        private readonly uint _webCheckRequestTimeout;

        /// <summary>URI for hang detection checking.</summary>
        private readonly string _webCheckRequestUri;

        /// <summary>Current hang count.</summary>
        private byte _hangCount;

        /// <summary>Wether current stop is planned.</summary>
        private bool _plannedStop;

        /// <summary>Actual process of the application.</summary>
        private Process _process;

        /// <summary>Standard error output stream (if any).</summary>
        private StreamWriter _stdErr;

        /// <summary>Standard output stream (if any).</summary>
        private StreamWriter _stdOut;

        /// <summary>Whether the stop was initiated.</summary>
        private bool _stopping;

        /// <summary>Hang detection timer.</summary>
        private Timer _timer;

        /// <summary>
        ///     Creates new instance of service application with provided configuration.
        /// </summary>
        /// <param name="app">Configuration of a service app.</param>
        public Application(SupervisorAppConfig app) {
            _startInfo = new ProcessStartInfo {
                FileName = app.Executable,
                Arguments = app.Arguments,
                UserName = app.User,
                WorkingDirectory = app.WorkingDirectory,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            if (!string.IsNullOrEmpty(app.ErrorFile)) {
                _stdErr = !File.Exists(app.ErrorFile)
                    ? File.CreateText(app.ErrorFile)
                    : new StreamWriter(app.ErrorFile, true);
                _startInfo.RedirectStandardError = true;
            }

            if (!string.IsNullOrEmpty(app.OutputFile)) {
                _stdErr = !File.Exists(app.OutputFile)
                    ? File.CreateText(app.OutputFile)
                    : new StreamWriter(app.OutputFile, true);
                _startInfo.RedirectStandardOutput = true;
            }

            if (app.UseHangDetection) {
                _webCheckRequestUri = app.WebCheckRequestUri;
                _webCheckRequestTimeout = app.WebCheckRequestTimeout;
                _startupTime = app.StartupTime;
                _interval = app.RecheckInterval;
                _recheckCount = app.RecheckCount;
            }

            _hangDetection = app.UseHangDetection;
        }

        /// <summary>
        ///     Dispose resources.
        /// </summary>
        public void Dispose() {
            Stop();
        }

        /// <summary>
        ///     Hang detection timer callback.
        /// </summary>
        /// <param name="state">The state.</param>
        private void TimerCallback(object state) {
            if (_stopping)
                return;

            var request = WebRequest.Create(_webCheckRequestUri);
            request.Timeout = (int) _webCheckRequestTimeout;

            try {
                var response = (HttpWebResponse) request.GetResponse();

                if (response.StatusCode != HttpStatusCode.OK) {
                    Log.Entry(Priority.Warning, "Web service `{0}` responded with error status `{1} - {2}`.",
                        _startInfo.FileName, (int) response.StatusCode, response.StatusDescription);
                }
            }
            catch (WebException) {
                _hangCount++;

                if (_hangCount >= _recheckCount) {
                    Restart();
                }
            }
        }

        /// <summary>
        ///     Service process exit event handler.
        /// </summary>
        /// <param name="sender">Sender info.</param>
        /// <param name="e">Event arguments.</param>
        private void Process_Exited(object sender, EventArgs e) {
            if (_stopping)
                return;

            if (!_plannedStop)
                Log.Entry(Priority.Warning, "Application `{0}` unexpectedly exited with code {1}, restarting.",
                    _startInfo.FileName, _process.ExitCode);

            Start();
        }

        /// <summary>
        ///     Service process error data (STDERR) received event handler.
        /// </summary>
        /// <param name="sender">Sender info.</param>
        /// <param name="e">Data event arguments.</param>
        private void Process_ErrorDataReceived(object sender, DataReceivedEventArgs e) {
            if (_stopping)
                return;

            if (!string.IsNullOrEmpty(e.Data))
                _stdErr.WriteLine(e.Data);
        }

        /// <summary>
        ///     Service process output data (STDOUT) received event handler.
        /// </summary>
        /// <param name="sender">Sender info.</param>
        /// <param name="e">Data event arguments.</param>
        private void Process_OutputDataReceived(object sender, DataReceivedEventArgs e) {
            if (_stopping)
                return;

            if (!string.IsNullOrEmpty(e.Data))
                _stdErr.WriteLine(e.Data);
        }

        /// <summary>
        ///     Start service application.
        /// </summary>
        public void Start() {
            _stopping = false;
            _process = new Process {StartInfo = _startInfo, EnableRaisingEvents = true};
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

        /// <summary>
        ///     Stop service application.
        ///     <remarks>Method is blocking.</remarks>
        /// </summary>
        public void Stop() {
            _stopping = true;

            if (_timer != null)
                _timer.Change(Timeout.Infinite, Timeout.Infinite);

            _plannedStop = true;

            if (_stdErr != null)
                _process.CancelErrorRead();

            if (_stdOut != null)
                _process.CancelOutputRead();

            if (!_process.HasExited) {
                // TODO: Implement graceful stop if possible.
                _process.Kill();
                _process.WaitForExit();
                _plannedStop = false;
            }

            _process = null;

            if (_stdErr != null) {
                _stdErr.Close();
                _stdErr.Dispose();
                _stdErr = null;
            }

            if (_stdOut != null) {
                _stdOut.Close();
                _stdOut.Dispose();
                _stdOut = null;
            }
        }

        /// <summary>
        ///     Restart service application.
        /// </summary>
        public void Restart() {
            Stop();
            _hangCount = 0;
            Start();
        }
    }
}