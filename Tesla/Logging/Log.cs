using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;

namespace Tesla.Logging
{
    public enum Priority
    {
        Emergency = 0,
        Alert = 1,
        Critical = 2,
        Error = 3,
        Warning = 4,
        Notice = 5,
        Info = 6,
        Debug = 7
    }

    public sealed class Log 
        : IDisposable
    {
        private readonly string _logName = "Log";
        private Stream _stream;
        private readonly bool _dontClose;
        private readonly StringBuilder _builder;
        private readonly bool _printThreadId;

        private static readonly Dictionary<Priority, string> Priorities = new Dictionary<Priority, string>
        {
            {Priority.Emergency, "emergency"},
            {Priority.Alert, "alert"},
            {Priority.Critical, "critical"},
            {Priority.Error, "error"},
            {Priority.Warning, "warning"},
            {Priority.Notice, "notice"},
            {Priority.Info, "info"},
            {Priority.Debug, "debug"}
        };

        public static Encoding Encoding { get; set; }
        public static string DefaultLogFile { get; set; }
        public static Stream DefaultLogStream { get; set; }

        private static Log _defaultLog;

        public static Log Default
        {
            get
            {
                return _defaultLog ?? (_defaultLog = DefaultLogStream != null
                    ? new Log(logStream: DefaultLogStream, printThreadId: true)
                    : new Log(logName: DefaultLogFile, printThreadId: true));
            }
        }

        public Log(string logName = null, Stream logStream = null, bool printThreadId = false)
        {
            if (!string.IsNullOrEmpty(logName))
            {
                _logName = logName;
            }

            if (logStream == null)
            {
                var assemblyDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

                if (assemblyDir == null)
                {
                    throw new ArgumentException("Cannot find executing assembly path.");
                }

                var path = Path.Combine(assemblyDir, _logName + ".log");

                try
                {
                    _stream = File.Create(path);
                    _dontClose = false;
                }
                catch (Exception e)
                {
                    throw new ArgumentException("Cannot create log file on executing assembly path.", e);
                }
            }
            else
            {
                _stream = logStream;
                _dontClose = true;
            }

            _printThreadId = printThreadId;
            Encoding = Encoding.UTF8;
            _builder = new StringBuilder();
        }

        public static void Entry(string message, params object[] args)
        {
            Entry(Priority.Info, message, args);
        }

        public static void Entry(Priority priority, string message, params object[] args)
        {
            Default.Write(priority, message, args);
        }

        public void Write(string message, params object[] args)
        {
            Write(Priority.Info, message, args);
        }

        public void Write(Priority priority, string message, params object[] args)
        {
            if (_stream == null)
            {
                throw new InvalidOperationException("There is no stream to write.");
            }

            if (Encoding == null)
            {
                Encoding = Encoding.UTF8;
            }

            _builder.Append(DateTime.Now);

            if (_logName != null)
            {
                _builder.Append(" [");
                _builder.Append(_logName);
                _builder.Append("] ");
            }

            _builder.Append("[");
            _builder.Append(Priorities[priority]);
            _builder.Append("] ");

            if (_printThreadId)
            {
                _builder.Append("[");
                _builder.Append(Thread.CurrentThread.ManagedThreadId);
                _builder.Append("] ");
            }

            _builder.AppendFormat(message, args);
            _builder.Append('\n');
            var buffer = Encoding.GetBytes(_builder.ToString());
            _stream.Write(buffer, 0, buffer.Length);
            _builder.Clear();
        }

        public void Dispose()
        {
            if (_stream != null && !_dontClose)
            {
                _stream.Close();
                _stream.Dispose();
                _stream = null;
            }
        }
    }
}
