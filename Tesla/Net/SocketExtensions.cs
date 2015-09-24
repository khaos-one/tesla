using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using Tesla.IO;

namespace Tesla.Net {
    /// <summary>
    ///     Расширения класса <see cref="Socket" />.
    /// </summary>
    public static class SocketExtensions {
        /// <summary>Максимальный разрешённый размер UDP-датаграммы.</summary>
        internal const int MaxUdpDatagramSize = 0x10000;

        /// <summary>
        ///     Производит асинхронное соединение с указанной конечной точкой.
        /// </summary>
        /// <param name="socket">Экземпляр класса сокета для которого производится соединение.</param>
        /// <param name="endPoint">Конечная точка для соединения.</param>
        /// <returns>Запускаемая асинхронная задача (<see cref="Task" />).</returns>
        public static Task ConnectAsync(this Socket socket, EndPoint endPoint) {
            return Task.Factory.FromAsync(socket.BeginConnect, socket.EndConnect, endPoint, null);
        }

        /// <summary>
        ///     Осуществляет асинхронный приём нового соединения.
        /// </summary>
        /// <param name="socket">Экземпляр сокета, для которого производится приём соединения.</param>
        /// <returns>
        ///     Запускаемая асинхронная задача (<see cref="Task" />), возвращающая новый сокет для обмена с удалённой
        ///     стороной.
        /// </returns>
        public static Task<Socket> AcceptAsync(this Socket socket) {
            return Task<Socket>.Factory.FromAsync(socket.BeginAccept, socket.EndAccept, null);
        }

        public static IAsyncResult BeginReceiveFrom(this Socket socket, AsyncCallback requestCallback) {
            EndPoint endPoint;

            switch (socket.AddressFamily) {
                case AddressFamily.InterNetwork:
                    endPoint = IPEndPointExtensions.Any;
                    break;
                default:
                    endPoint = IPEndPointExtensions.IPv6Any;
                    break;
            }

            var buffer = ProgramBuffer.Manager.TakeBuffer(MaxUdpDatagramSize);
            return socket.BeginReceiveFrom(buffer, 0, buffer.Length, SocketFlags.None, ref endPoint, requestCallback,
                buffer);
        }

        public static byte[] EndReceiveFrom(this Socket socket, IAsyncResult asyncResult, ref IPEndPoint remoteEndPoint) {
            EndPoint endPoint;

            switch (socket.AddressFamily) {
                case AddressFamily.InterNetwork:
                    endPoint = IPEndPointExtensions.Any;
                    break;
                default:
                    endPoint = IPEndPointExtensions.IPv6Any;
                    break;
            }

            var received = socket.EndReceiveFrom(asyncResult, ref endPoint);
            remoteEndPoint = (IPEndPoint) endPoint;

            var buffer = (byte[]) asyncResult.AsyncState;
            var returnBuffer = new byte[received];

            Buffer.BlockCopy(buffer, 0, returnBuffer, 0, returnBuffer.Length);
            ProgramBuffer.Manager.ReturnBuffer(buffer);

            return returnBuffer;
        }

        /// <summary>
        ///     Производит асинхронный приём UDP-датаграммы.
        /// </summary>
        /// <param name="socket">Сокет, через который производится приём датаграммы.</param>
        /// <returns>
        ///     Запускаемая асинхронная задача (<see cref="Task" />), возвращающая кортеж из содержимого датаграммы и
        ///     удалённой конечной точки для связи.
        /// </returns>
        public static Task<Tuple<byte[], IPEndPoint>> ReceiveAsync(this Socket socket) {
            return Task<Tuple<byte[], IPEndPoint>>.Factory.FromAsync(
                (callback, state) => socket.BeginReceiveFrom(callback),
                ar => {
                    IPEndPoint remoteIPEndPoint = null;
                    var received = socket.EndReceiveFrom(ar, ref remoteIPEndPoint);
                    return new Tuple<byte[], IPEndPoint>(received, remoteIPEndPoint);
                }, null);
        }

        /// <summary>
        ///     Получает сетевой поток из сокета.
        /// </summary>
        /// <param name="socket">Сокет, для которого получается поток.</param>
        /// <returns>Сетевой поток для обмена.</returns>
        public static NetworkStream GetStream(this Socket socket) {
            return new NetworkStream(socket);
        }

        /// <summary>
        ///     Считывает из сокета текущие данные побайтно, возвращая результат при превышении лимита ожидания.
        /// </summary>
        /// <param name="socket">Сокет, для которого производится чтение.</param>
        /// <returns>Массив данных, считаных из сокета.</returns>
        public static byte[] ReadToTimeout(this Socket socket) {
            using (var ns = socket.GetStream()) {
                return ns.ReadToTimeout();
            }
        }

        /// <summary>
        ///     Выполняет poll-чтение из сокета.
        /// </summary>
        /// <param name="socket">Сокет, для которого производится чтение.</param>
        /// <param name="buffer">Буфер для записи результата.</param>
        /// <param name="microsecondsTimeout">Время ожидания завершения операции в микросекундах.</param>
        /// <returns>Считанное количество данных.</returns>
        public static int PollRead(this Socket socket, byte[] buffer, int microsecondsTimeout) {
            if (socket.Poll(microsecondsTimeout, SelectMode.SelectRead)) {
                return socket.Receive(buffer);
            }

            return -1;
        }

        /// <summary>
        ///     Выполняет реальную проверку, является ли сокет действительно подключенным.
        /// </summary>
        /// <param name="socket">Сокет, для которого производится проверка.</param>
        /// <param name="timeout">Общий максимальный таймаут операции.</param>
        /// <returns>Значение, показывающее, подключен ли сокет.</returns>
        public static bool IsConnected(this Socket socket, int timeout = 100) {
            return !((socket.Poll(timeout, SelectMode.SelectRead) && (socket.Available == 0)) || !socket.Connected);
        }
    }
}