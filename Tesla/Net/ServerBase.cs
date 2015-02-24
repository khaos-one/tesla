using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

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
        protected abstract Task<Action> AcceptClient();

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

            #pragma warning disable 4014
            ListenAsync();
            #pragma warning restore 4014
        }

        /// <summary>
        /// Асинхронный метод, выполняющий ожидание новых соединений и запуск функций-обработчиков.
        /// Вызывается рекурсивно внутри себя.
        /// </summary>
        protected async Task ListenAsync()
        {
            while (true)
            {
                if (_cts.Token.IsCancellationRequested)
                {
                    return;
                }

                var accept = await AcceptClient();
                ThreadPool.QueueUserWorkItem(_ => accept());
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
                Trace.TraceWarning("Server stop exception: {0}.", e);
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
