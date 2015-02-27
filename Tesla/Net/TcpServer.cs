using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Tesla.Net
{
    public abstract class TcpServer
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

        /// <summary>Сокет, принимающий новые подключения.</summary>
        protected Socket ListenerSocket;
        /// <summary>Обработчик входящих соединений.</summary>

        /// <summary>Локальная конечная точка (IP-адрес и порт), на которой в данный момент работает сервер.</summary>
        public IPEndPoint LocalEndPoint { get; protected set; }

        /// <summary>Таймаут операции записи для подключений.</summary>
        public int? ClientSocketSendTimeout { get; set; }
        /// <summary>Таймаут операции чтения для подключений.</summary>
        public int? ClientSocketReceiveTimeout { get; set; }

        /// <summary>
        /// Создаёт новый экземпляр класса сервера с указанными параметрами.
        /// </summary>
        /// <param name="ip">IP-адрес сервера (если на целевой машине больше одного сетевого интерфейса).</param>
        /// <param name="port">Порт сервера.</param>
        protected TcpServer(IPAddress ip, int port)
        {
            LocalEndPoint = new IPEndPoint(ip, port);
            ListenerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            // Trying to avoid sporadic connection resets.
            ListenerSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, false);
            ListenerSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.DontLinger, true);
        }

        /// <summary>
        /// Создаёт новый экземпляр класса сервера с указанными параметрами.
        /// </summary>
        /// <param name="port">Порт сервера.</param>
        protected TcpServer(int port)
            : this(IPAddress.Any, port)
        { }

        /// <summary>
        /// Порт, на котором в данный момент работает сервер.
        /// Свойство доступно только для чтения.
        /// </summary>
        public int Port
        {
            get { return LocalEndPoint.Port; }
        }

        /// <summary>
        /// Абстрактный метод, реализующий действия при запуске сервера.
        /// </summary>
        protected void OnStart()
        {
            try
            {
                ListenerSocket.Bind(LocalEndPoint);
                ListenerSocket.Listen(500);

                LocalEndPoint = (IPEndPoint)ListenerSocket.LocalEndPoint;
            }
            catch (SocketException) { /* TODO: Process exception. */ }
            catch (ObjectDisposedException) { /* TODO: Process exception. */ }

            // Inform log that we have started.
            Trace.TraceInformation("[TcpServer] [{0}] Server started on port {1}.", ServerName, Port);
        }

        /// <summary>
        /// Абстрактный метод, реализующий действия при останове сервера.
        /// </summary>
        protected void OnStop()
        {
            ListenerSocket.Close();
            //ListenerSocket.Disconnect(true);

            // Inform log that we have stopped.
            Trace.TraceInformation("[TcpServer] [{0}] Server stopped on port {1}.", ServerName, Port);
        }

        protected abstract void HandleRequest(Socket socket);
        protected abstract void HandleException(Socket socket, Exception e);

        /// <summary>
        /// Абстрактный класс, реализующий процедуру ожидания и обработки подключения нового соединения с сервером.
        /// Возвращает анонимную функцию-обработчик, которая затем будет выполнена в пуле потоков.
        /// </summary>
        /// <returns>Анонимная функция-обработчик данного соединения.</returns>
        protected void AcceptClient(object state)
        {
            var socket = (Socket)state;

            if (!socket.Connected)
            {
                Disconnect(socket);
                return;
            }

            try
            {
                HandleRequest(socket);
            }
            catch (Exception e)
            {
                Trace.TraceWarning("[TcpServer] [{0}] TCP Handler exception: {1}.", ServerName, e);
                HandleException(socket, e);
            }
            finally
            {
                Disconnect(socket);
            }
        }

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

        /// <summary>
        /// Метод, выполняющий ожидание новых соединений и запуск функций-обработчиков.
        /// Вызывается рекурсивно внутри себя.
        /// </summary>
        protected void Listen()
        {
            while (true)
            {
                if (_cts.Token.IsCancellationRequested)
                {
                    return;
                }

                var socket = ListenerSocket.Accept();

                // Temporal; to detect server thread hangs.
                if (_runningThreadsCount > 20)
                {
                    Trace.TraceWarning("[ServerBase] [{0}] Currently running {1} threads. Possible thread hang slam.", ServerName, _runningThreadsCount);
                }

                ThreadPool.QueueUserWorkItem(AcceptClient, socket);
            }
        }

        /// <summary>
        /// Отсоединяет и закрывает указанный сокет и поток.
        /// </summary>
        /// <param name="socket">Сокет по которому разрывается соединение.</param>
        private static void Disconnect(Socket socket)
        {
            try
            {
                if (socket != null)
                {
                    //socket.Shutdown(SocketShutdown.Both);
                    socket.Close();
                    socket.Dispose();
                }
            }
            catch (Exception e)
            {
                Trace.TraceWarning("[TcpServer] TCP closure exception: {0}.", e.Message);
            }

            // Other logic was added.
            //socket.Shutdown(SocketShutdown.Both);

            //try
            //{
            //    socket.Close(1000);
            //    socket.Dispose();
            //}
            //catch (Exception e)
            //{
            //    Trace.TraceWarning("TCP closure exception: {0}.", e.Message);
            //}
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
