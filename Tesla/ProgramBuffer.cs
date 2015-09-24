using System.ServiceModel.Channels;

namespace Tesla {
    public static class ProgramBuffer {
        private const int BufferPoolSize = 0x1000000;
        private const int BufferSize = 0x10000;

        private static BufferManager _manager;

        public static BufferManager Manager
            => _manager ?? (_manager = BufferManager.CreateBufferManager(BufferPoolSize, BufferSize));
    }
}