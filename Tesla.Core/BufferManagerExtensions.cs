using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;

namespace Tesla
{
    public static class BufferManagerExtensions
    {
        private const int BufferPoolSize = 10000000;
        private const int BufferSize = 100000;

        private static BufferManager _manager;

        public static BufferManager Instance()
        {
            return _manager ?? (_manager = BufferManager.CreateBufferManager(BufferPoolSize, BufferSize));
        }
    }
}
