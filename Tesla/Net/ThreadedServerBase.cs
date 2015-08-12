using System;
using System.Collections.Generic;
using System.Threading;
using Tesla.Logging;

namespace Tesla.Net
{
    /// <summary>
    /// Абстрактный базовый класс для реализации параллельных серверов.
    /// </summary>
    public abstract class ThreadedServerBase
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
        /// <summary>Поток, "слушающий" входящие соединения.</summary>
        private Thread _listenerThread;

        protected bool IsCancellationRequested
        {
            get { return _cts.Token.IsCancellationRequested; }
        }

        /// <summary>
        /// Абстрактный метод, реализующий действия при запуске сервера.
        /// </summary>
        protected abstract void OnStart();

        /// <summary>
        /// Абстрактный метод, реализующий действия при останове сервера.
        /// </summary>
        protected abstract void OnStop();

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

            _listenerThread = new Thread(Listen) { IsBackground = true };
            _listenerThread.Start();
        }

        protected abstract object AcceptClient();
        protected abstract void HandleClient(object obj);

        /// <summary>
        /// Абстрактный метод, выполняющий ожидание новых соединений и запуск функций-обработчиков.
        /// Вызывается рекурсивно внутри себя.
        /// </summary>
        protected void Listen()
        {
            while (true)
            {
                if (IsCancellationRequested)
                {
                    return;
                }

                try
                {
                    var obj = AcceptClient();

                    if (obj == null)
                        continue;

                    ThreadPool.QueueUserWorkItem(HandleClient, obj);
                }
                catch (ObjectDisposedException e)
                {
                    return;
                }
                catch (Exception e)
                {
                    Log.Entry(Priority.Warning, "[ThreadedServerBase] [{0}] Listener exception: {1}", ServerName, e);
                }
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
                Log.Entry(Priority.Warning, "[ServerBase] [{1}] Server stop exception: {0}.", ServerName, e);
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
