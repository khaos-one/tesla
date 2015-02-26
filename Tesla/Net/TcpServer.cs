using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;

namespace Tesla.Net
{
    /// <summary>
    /// Представляет TCP-сервер.
    /// </summary>
    public class TcpServer
        : SocketServerBase<Action<Socket>, Func<Socket, Exception, bool>>
    {
        /// <summary>Таймаут операции записи для подключений.</summary>
        public int? ClientSocketSendTimeout { get; set; }
        /// <summary>Таймаут операции чтения для подключений.</summary>
        public int? ClientSocketReceiveTimeout { get; set; }

        /// <summary>
        /// Создаёт новый экземпляр класса TCP-сервера с указанными параметрами.
        /// </summary>
        /// <param name="handlerFunc">Функция-обработчик входящих соединений.</param>
        /// <param name="ip">IP-адрес сервера (если на целевой машине больше одного сетевого интерфейса).</param>
        /// <param name="port">Порт TCP-сервера.</param>
        /// <param name="errorFunc">Функция-обработчик ошибок.</param>
        public TcpServer(Action<Socket> handlerFunc, IPAddress ip, int port, Func<Socket, Exception, bool> errorFunc)
            : base(handlerFunc, ip, port, errorFunc)
        {
            ListenerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            // Trying to avoid sporadic connection resets.
            ListenerSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, false);
            ListenerSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.DontLinger, true);
        }

        /// <summary>
        /// Создаёт новый экземпляр класса TCP-сервера с указанными параметрами.
        /// </summary>
        /// <param name="handlerFunc">Функция-обработчик входящих соединений.</param>
        /// <param name="ip">IP-адрес сервера (если на целевой машине больше одного сетевого интерфейса).</param>
        /// <param name="port">Порт TCP-сервера.</param>
        public TcpServer(Action<Socket> handlerFunc, IPAddress ip, int port)
            : this(handlerFunc, ip, port, null)
        { }

        /// <summary>
        /// Создаёт новый экземпляр класса TCP-сервера с указанными параметрами.
        /// </summary>
        /// <param name="handlerFunc">Функция-обработчик входящих соединений.</param>
        /// <param name="port">Порт TCP-сервера.</param>
        public TcpServer(Action<Socket> handlerFunc, int port)
            : this(handlerFunc, IPAddress.Any, port)
        { }

        /// <summary>
        /// Создаёт новый экземпляр класса TCP-сервера с указанными параметрами.
        /// </summary>
        /// <param name="handlerFunc">Функция-обработчик входящих соединений.</param>
        public TcpServer(Action<Socket> handlerFunc)
            : this(handlerFunc, 0)
        { }

        /// <summary>
        /// Связывает сокет сервера.
        /// </summary>
        protected override void BindSocket()
        {
            ListenerSocket.Bind(LocalEndPoint);
            ListenerSocket.Listen(500);

            LocalEndPoint = (IPEndPoint)ListenerSocket.LocalEndPoint;
        }

        /// <summary>
        /// Слушает сокет сервера на наличие новых соединений, принимает их по мере поступления.
        /// </summary>
        /// <returns>Анонимная функция-обработчик принятого соединения.</returns>
        protected override Action AcceptClient()
        {
            // Accept incoming connection.
            var socket = ListenerSocket.Accept();

            // Set new socket options.
            // Trying to avoid sporadic connection resets with KeepAlive=false and disabling Nagle algorithm (DontLinger=true).
            socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, false);
            socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.DontLinger, true);

            // Set client socket timeouts.
            if (ClientSocketSendTimeout != null)
            {
                socket.SendTimeout = ClientSocketSendTimeout.Value;
            }

            if (ClientSocketReceiveTimeout != null)
            {
                socket.ReceiveTimeout = ClientSocketReceiveTimeout.Value;
            }

            return () =>
            {
                if (!socket.Connected)
                {
                    Disconnect(socket);
                    return;
                }

                try
                {
                    HandlerFunc(socket);
                }
                catch (Exception e)
                {
                    Trace.TraceWarning("[TcpServer] [{0}] TCP Handler exception: {1}.", ServerName, e);

                    if (ExceptionHandlerFunc != null)
                    {
                        ExceptionHandlerFunc(socket, e);
                    }
                }
                finally
                {
                    Disconnect(socket);
                }
            };
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
        }
    }
}
