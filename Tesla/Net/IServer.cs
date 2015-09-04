using System;

namespace Tesla.Net
{
    /// <summary>
    /// Общий интерфейс сервера.
    /// </summary>
    public interface IServer
        : IDisposable
    {
        /// <summary>
        /// Запуск сервера.
        /// </summary>
        void Start();

        /// <summary>
        /// Остановка сервера.
        /// </summary>
        void Stop();

        /// <summary>
        /// Ожидать останова сервера.
        /// </summary>
        void WaitForJoin();
    }
}
