using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;

namespace Tesla.Net
{
    /// <summary>
    /// Абстрактный базовый класс для реализации сокет-серверов.
    /// </summary>
    /// <typeparam name="THandler">Тип обработчика входящих соединений.</typeparam>
    /// <typeparam name="TExceptionHandler">Тип обработчика ошибок.</typeparam>
    public abstract class SocketServerBase<THandler, TExceptionHandler>
        : ThreadedServerBase
        where TExceptionHandler : class
    {
        /// <summary>Сокет, принимающий новые подключения.</summary>
        protected Socket ListenerSocket;
        /// <summary>Обработчик входящих соединений.</summary>
        protected readonly THandler HandlerFunc;
        /// <summary>Обработчик ошибок.</summary>
        protected readonly TExceptionHandler ExceptionHandlerFunc;

        /// <summary>Локальная конечная точка (IP-адрес и порт), на которой в данный момент работает сервер.</summary>
        public IPEndPoint LocalEndPoint { get; protected set; }

        /// <summary>
        /// Создаёт новый экземпляр класса сервера с указанными параметрами.
        /// </summary>
        /// <param name="handlerFunc">Обработчик входящих соединений.</param>
        /// <param name="ip">IP-адрес сервера (если на целевой машине больше одного сетевого интерфейса).</param>
        /// <param name="port">Порт сервера.</param>
        /// <param name="exceptionHandlerFunc">Обработчик ошибок.</param>
        protected SocketServerBase(THandler handlerFunc, IPAddress ip, int port, TExceptionHandler exceptionHandlerFunc)
        {
            HandlerFunc = handlerFunc;
            LocalEndPoint = new IPEndPoint(ip, port);
            ExceptionHandlerFunc = exceptionHandlerFunc;
        }

        /// <summary>
        /// Создаёт новый экземпляр класса сервера с указанными параметрами.
        /// </summary>
        /// <param name="handlerFunc">Обработчик входящих соединений.</param>
        /// <param name="ip">IP-адрес сервера (если на целевой машине больше одного сетевого интерфейса).</param>
        /// <param name="port">Порт сервера.</param>
        protected SocketServerBase(THandler handlerFunc, IPAddress ip, int port)
            : this(handlerFunc, ip, port, null)
        { }

        /// <summary>
        /// Создаёт новый экземпляр класса сервера с указанными параметрами.
        /// </summary>
        /// <param name="handlerFunc">Обработчик входящих соединений.</param>
        /// <param name="port">Порт сервера.</param>
        protected SocketServerBase(THandler handlerFunc, int port)
            : this(handlerFunc, IPAddress.Any, port)
        { }

        /// <summary>
        /// Создаёт новый экземпляр класса сервера с указанными параметрами.
        /// </summary>
        /// <param name="handlerFunc">Обработчик входящих соединений.</param>
        protected SocketServerBase(THandler handlerFunc)
            : this(handlerFunc, 0)
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
        /// Абстрактный метод, реализующий процедуру связывания для сокета сервера.
        /// </summary>
        protected abstract void BindSocket();

        /// <summary>
        /// Метод, выполняющийся во время запуска сервера.
        /// </summary>
        protected override void OnStart()
        {
            try
            {
                BindSocket();
            }
            catch (SocketException) { /* TODO: Process exception. */ }
            catch (ObjectDisposedException) { /* TODO: Process exception. */ }

            // Inform log that we have started.
            Trace.TraceInformation("[TcpServer] [{0}] Server started on port {1}.", ServerName, Port);
        }

        /// <summary>
        /// Метод, выполняющийся во время остановки сервера.
        /// </summary>
        protected override void OnStop()
        {
            ListenerSocket.Close();
            //ListenerSocket.Disconnect(true);

            // Inform log that we have stopped.
            Trace.TraceInformation("[TcpServer] [{0}] Server stopped on port {1}.", ServerName, Port);
        }

        /// <summary>
        /// Останавливает сервер и освобождает задействованные ресурсы.
        /// </summary>
        public new void Dispose()
        {
            base.Dispose();

            if (ListenerSocket != null)
            {
                ListenerSocket.Close();
                ListenerSocket.Dispose();
            }
        }
    }
}
