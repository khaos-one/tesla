using System.Threading;

namespace Tesla
{
    public static class Daemonizer
    {
        public static void Daemonize(ThreadStart start)
        {
            var thread = new Thread(start);
            thread.IsBackground = true;
            thread.Start();
            //Thread.CurrentThread.Abort();
        }

        public static void Daemonize(ParameterizedThreadStart start, object state)
        {
            var thread = new Thread(start);
            thread.IsBackground = true;
            thread.Start(state);
            //Thread.CurrentThread.Abort();
        }
    }
}
