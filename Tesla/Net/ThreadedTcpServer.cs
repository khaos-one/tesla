using System;
using System.Net;
using System.Net.Sockets;
using Tesla.Logging;

namespace Tesla.Net
{
    /// <summary>
    /// Абстрактный класс, реализующий потоковый TCP-сервер.
    /// </summary>
    public abstract class ThreadedTcpServer
        : ThreadedServerBase
    {
        /// <summary>Сокет, принимающий новые подключения.</summary>
        protected Socket ListenerSocket;

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
        protected ThreadedTcpServer(IPAddress ip, int port)
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
        protected ThreadedTcpServer(int port)
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
        /// Метод, реализующий действия при запуске сервера.
        /// </summary>
        protected override void OnStart()
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
            Log.Entry("[TcpServer] [{0}] Server started on port {1}.", ServerName, Port);
        }

        /// <summary>
        /// Метод, реализующий действия при останове сервера.
        /// </summary>
        protected override void OnStop()
        {
            ListenerSocket.Close();
            //ListenerSocket.Disconnect(true);

            // Inform log that we have stopped.
            Log.Entry("[TcpServer] [{0}] Server stopped on port {1}.", ServerName, Port);
        }

        /// <summary>
        /// Абстрактный метод, реализующий функционал обработки запроса клиента.
        /// </summary>
        /// <param name="socket">Сокет клиента.</param>
        protected abstract void HandleConnection(Socket socket);

        /// <summary>
        /// Абстрактный метод, реализующий функционал обработки ошибки при обработке запроса от клиента.
        /// </summary>
        /// <param name="socket">Сокет клиента.</param>
        /// <param name="e">Ошибка.</param>
        protected abstract void HandleException(Socket socket, Exception e);

        /// <summary>
        /// Отсоединяет и закрывает указанный сокет и поток.
        /// </summary>
        /// <param name="socket">Сокет по которому разрывается соединение.</param>
        private static void Disconnect(Socket socket)
        {
            try
            {
                if (socket != null && socket.Connected)
                {
                    //socket.Shutdown(SocketShutdown.Both);
                    socket.Close();
                    socket.Dispose();
                }
            }
            catch (Exception e)
            {
                Log.Entry(Priority.Warning, "[TcpServer] TCP closure exception: {0}.", e.Message);
            }
        }

        protected override object AcceptClient()
        {
            return ListenerSocket.Accept();
        }

        /// <summary>
        /// Метод, реализующий процедуру ожидания и обработки подключения нового соединения с сервером.
        /// Возвращает анонимную функцию-обработчик, которая затем будет выполнена в пуле потоков.
        /// </summary>
        /// <returns>Анонимная функция-обработчик данного соединения.</returns>
        protected override void HandleClient(object state)
        {
            var socket = (Socket) state;

            if (!socket.Connected)
            {
                Disconnect(socket);
                return;
            }

            try
            {
                HandleConnection(socket);
            }
            catch (Exception e)
            {
                Log.Entry(Priority.Warning, "[TcpServer] [{0}] TCP Handler exception: {1}.", ServerName, e);
                HandleException(socket, e);
            }
            finally
            {
                Disconnect(socket);
            }
        }
    }
}
