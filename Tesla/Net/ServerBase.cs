using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace Tesla.Net
{
    /// <summary>
    /// Абстрактный базовый класс для реализации параллельных серверов.
    /// </summary>
    public abstract class ServerBase
        : IServer
    {
        /// <summary>Объект синхронизации потоков обработчиков при необходимости остановки сервера.</summary>
        private readonly CancellationTokenSource _cts = new CancellationTokenSource();
        /// <summary>Флаг, показывающий включен ли в данный момент сервер. Используется для многопоточной синхронизации.</summary>
        private int _started;
        /// <summary>Флаг, показывающий выключен ли в данный момент сервер. Используется для многопоточной синхронизации.</summary>
        private int _stopped;
        /// <summary>Обработчики ожидания для всех потоков обработчиков.</summary>
        private List<WaitHandle> _threadPoolHandles;
        /// <summary>Счётчик запущенных в данный момент потоков.</summary>
        private int _runningThreadsCount = 0;

        private Thread _listenerThread;

        /// <summary>
        /// Абстрактный метод, реализующий действия при запуске сервера.
        /// </summary>
        protected abstract void OnStart();

        /// <summary>
        /// Абстрактный метод, реализующий действия при останове сервера.
        /// </summary>
        protected abstract void OnStop();

        /// <summary>
        /// Абстрактный класс, реализующий процедуру ожидания и обработки подключения нового соединения с сервером.
        /// Возвращает анонимную функцию-обработчик, которая затем будет выполнена в пуле потоков.
        /// </summary>
        /// <returns>Анонимная функция-обработчик данного соединения.</returns>
        protected abstract Action AcceptClient();

        /// <summary>
        /// Имя сервера.
        /// </summary>
        public string ServerName { get; set; }

        /// <summary>
        /// Запускает сервер.
        /// </summary>
        public void Start()
        {
            if (Interlocked.CompareExchange(ref _started, 1, 0) != 0)
            {
                throw new InvalidOperationException("Server has already been started.");
            }

            OnStart();

            _listenerThread = new Thread(ListenAsync) { IsBackground = true };
            _listenerThread.Start();
        }

        /// <summary>
        /// Метод, выполняющий ожидание новых соединений и запуск функций-обработчиков.
        /// Вызывается рекурсивно внутри себя.
        /// </summary>
        protected void ListenAsync()
        {
            while (true)
            {
                if (_cts.Token.IsCancellationRequested)
                {
                    return;
                }

                var accept = AcceptClient();

                // Temporal; to detect server thread hangs.
                if (_runningThreadsCount > 20)
                {
                    Trace.TraceWarning("[ServerBase] [{0}] Currently running {1} threads. Possible thread hang slam.", ServerName, _runningThreadsCount);
                }

                ThreadPool.QueueUserWorkItem(_ =>
                {
                    try
                    {
                        Interlocked.Increment(ref _runningThreadsCount);
                        accept();
                    }
                    finally
                    {
                        Interlocked.Decrement(ref _runningThreadsCount);
                    }
                });
            }
        }

        /// <summary>
        /// Останавливает сервер.
        /// </summary>
        public void Stop()
        {
            if (_started == 0)
            {
                return;
            }

            if (Interlocked.CompareExchange(ref _stopped, 1, 0) != 0)
            {
                return;
            }

            try
            {
                _cts.Cancel();
                OnStop();
            }
            catch (Exception e)
            {
                Trace.TraceWarning("[ServerBase] [{1}] Server stop exception: {0}.", ServerName, e);
            }
        }

        /// <summary>
        /// Останавливает сервер и освобождает задействованные ресурсы.
        /// </summary>
        public void Dispose()
        {
            Stop();
        }
    }
}
