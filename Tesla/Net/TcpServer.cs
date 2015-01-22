using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Tesla.Net
{
    using HandlerFunc = Func<Socket, Task>;
    using ErrorFunc = Func<Socket, Exception, bool>;

    /// <summary>
    /// Представляет TCP-сервер.
    /// </summary>
    public class TcpSocketServer
        : SocketServerBase<HandlerFunc, ErrorFunc>
    {
        /// <summary>
        /// Создаёт новый экземпляр класса TCP-сервера с указанными параметрами.
        /// </summary>
        /// <param name="handlerFunc">Функция-обработчик входящих соединений.</param>
        /// <param name="ip">IP-адрес сервера (если на целевой машине больше одного сетевого интерфейса).</param>
        /// <param name="port">Порт TCP-сервера.</param>
        /// <param name="errorFunc">Функция-обработчик ошибок.</param>
        public TcpSocketServer(HandlerFunc handlerFunc, IPAddress ip, int port, ErrorFunc errorFunc)
            : base(handlerFunc, ip, port, errorFunc)
        {
            ListenerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //ListenerSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);
            //ListenerSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.DontLinger, true);
        }

        /// <summary>
        /// Создаёт новый экземпляр класса TCP-сервера с указанными параметрами.
        /// </summary>
        /// <param name="handlerFunc">Функция-обработчик входящих соединений.</param>
        /// <param name="ip">IP-адрес сервера (если на целевой машине больше одного сетевого интерфейса).</param>
        /// <param name="port">Порт TCP-сервера.</param>
        public TcpSocketServer(HandlerFunc handlerFunc, IPAddress ip, int port)
            : this(handlerFunc, ip, port, null)
        { }

        /// <summary>
        /// Создаёт новый экземпляр класса TCP-сервера с указанными параметрами.
        /// </summary>
        /// <param name="handlerFunc">Функция-обработчик входящих соединений.</param>
        /// <param name="port">Порт TCP-сервера.</param>
        public TcpSocketServer(HandlerFunc handlerFunc, int port) 
            : this(handlerFunc, IPAddress.Any, port)
        { }

        /// <summary>
        /// Создаёт новый экземпляр класса TCP-сервера с указанными параметрами.
        /// </summary>
        /// <param name="handlerFunc">Функция-обработчик входящих соединений.</param>
        public TcpSocketServer(HandlerFunc handlerFunc) 
            : this(handlerFunc, 0)
        { }

        /// <summary>
        /// Связывает сокет сервера.
        /// </summary>
        protected override void BindSocket()
        {
            ListenerSocket.Bind(LocalEndPoint);
            ListenerSocket.Listen(500);

            LocalEndPoint = (IPEndPoint) ListenerSocket.LocalEndPoint;
        }

        /// <summary>
        /// Слушает сокет сервера на наличие новых соединений, принимает их по мере поступления.
        /// </summary>
        /// <returns>Анонимная функция-обработчик принятого соединения.</returns>
        protected override async Task<Action> AcceptClient()
        {
            var socket = await ListenerSocket.AcceptAsync();
            return async () =>
            {
                if (!socket.Connected)
                {
                    Disconnect(socket);
                    return;
                }

                try
                {
                    await HandlerFunc(socket);
                }
                catch (Exception e)
                {
                    Trace.TraceWarning("TCP Handler exception: {0}.", e);

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
        /// <param name="stream">Поток этого сокета (если есть).</param>
        private static void Disconnect(Socket socket, Stream stream = null)
        {
            try
            {
                if (stream != null)
                {
                    stream.Close();
                }

                if (socket != null)
                {
                    socket.Shutdown(SocketShutdown.Both);
                    socket.Close();
                }
            }
            catch (Exception e)
            {
                Trace.TraceWarning("TCP closure exception: {0}.", e.Message);
            }
        }
    }
}
