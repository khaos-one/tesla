﻿//-----------------------------------------------------------------------------------------
// <author>Egor 'khaos' Zelensky <i@khaos.su></author>
// <description>
//    This file originates from 
//    <a href="https://github.com/khaos-one/tesla/tree/master/Tesla">Tesla</a>
//    library.
// </description>
//-----------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading;
using Tesla.Logging;

namespace Tesla.Net {
    /// <summary>
    ///     Абстрактный базовый класс для реализации параллельных серверов.
    /// </summary>
    public abstract class ThreadedServerBase
        : IServer {
        /// <summary>Объект синхронизации потоков обработчиков при необходимости остановки сервера.</summary>
        private readonly CancellationTokenSource _cts = new CancellationTokenSource();

        private readonly ManualResetEventSlim _evt = new ManualResetEventSlim();

        /// <summary>Поток, "слушающий" входящие соединения.</summary>
        private Thread _listenerThread;

        /// <summary>Счётчик запущенных в данный момент потоков.</summary>
        private int _runningThreadsCount = 0;

        /// <summary>Флаг, показывающий включен ли в данный момент сервер. Используется для многопоточной синхронизации.</summary>
        private int _started;

        /// <summary>Флаг, показывающий выключен ли в данный момент сервер. Используется для многопоточной синхронизации.</summary>
        private int _stopped;

        /// <summary>Обработчики ожидания для всех потоков обработчиков.</summary>
        private List<WaitHandle> _threadPoolHandles;

        protected bool IsCancellationRequested => _cts.Token.IsCancellationRequested;

        /// <summary>
        ///     Имя сервера.
        /// </summary>
        public string ServerName { get; set; }

        /// <summary>
        ///     Запускает сервер.
        /// </summary>
        public void Start() {
            if (Interlocked.CompareExchange(ref _started, 1, 0) != 0) {
                throw new InvalidOperationException("Server has already been started.");
            }

            OnStart();

            _listenerThread = new Thread(Listen) { IsBackground = true };
            _listenerThread.Start();
            _evt.Reset();
        }

        /// <summary>
        ///     Останавливает сервер.
        /// </summary>
        public void Stop() {
            /*if (_started == 0) {
                return;
            }*/

            if (Interlocked.CompareExchange(ref _started, 0, 1) != 1) {
                return;
            }

            try {
                _cts.Cancel();
                OnStop();
            } catch (Exception e) {
                Log.Entry(Priority.Warning, "[ServerBase] [{1}] Server stop exception: {0}.", ServerName, e);
            } finally {
                _evt.Set();
            }
        }

        /// <summary>
        ///     Останавливает сервер и освобождает задействованные ресурсы.
        /// </summary>
        public void Dispose() {
            Stop();
        }

        public void WaitForJoin() {
            _evt.Wait();
        }

        /// <summary>
        ///     Абстрактный метод, реализующий действия при запуске сервера.
        /// </summary>
        protected abstract void OnStart();

        /// <summary>
        ///     Абстрактный метод, реализующий действия при останове сервера.
        /// </summary>
        protected abstract void OnStop();

        protected abstract object AcceptClient();
        protected abstract void HandleClient(object obj);

        /// <summary>
        ///     Абстрактный метод, выполняющий ожидание новых соединений и запуск функций-обработчиков.
        ///     Вызывается рекурсивно внутри себя.
        /// </summary>
        protected void Listen() {
            while (true) {
                if (IsCancellationRequested) {
                    return;
                }

                try {
                    var obj = AcceptClient();

                    if (obj == null)
                        continue;

                    ThreadPool.QueueUserWorkItem(HandleClient, obj);
                } catch (ObjectDisposedException) {
                    return;
                } catch (Exception e) {
                    Log.Entry(Priority.Warning, "[ThreadedServerBase] [{0}] Listener exception: {1}", ServerName, e);
                }
            }
        }
    }
}