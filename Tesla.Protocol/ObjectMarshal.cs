using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Tesla.Protocol
{
    public static class ObjectMarshal
    {
        public static byte[] BytesFromStructure<T>(T structure)
            where T : struct
        {
            var size = Marshal.SizeOf(structure);
            var buffer = new Byte[size];
            var ptr = Marshal.AllocHGlobal(size);

            Marshal.StructureToPtr(structure, ptr, true);
            Marshal.Copy(ptr, buffer, 0, size);
            Marshal.FreeHGlobal(ptr);

            return buffer;
        }

        public static T StructureFromBytes<T>(byte[] data)
            where T : struct
        {
            var ptr = GCHandle.Alloc(data, GCHandleType.Pinned);
            var result = (T) Marshal.PtrToStructure(ptr.AddrOfPinnedObject(), typeof (T));
            ptr.Free();

            return result;
        }
    }
}
