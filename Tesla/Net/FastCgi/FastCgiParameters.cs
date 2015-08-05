using System;

namespace Tesla.Net.FastCgi
{
    public class FastCgiParameters
    {
        public uint NameLength;
        public uint ValueLength;
        public string Name;
        public string Value;
        public bool IsEnd;

        public FastCgiParameters(FastCgiRecord record)
        {
            if (record.Type != FastCgiRecord.RecordType.Params)
                throw new ArgumentException("record");

            if (record.ContentLength == 0)
            {
                NameLength = 0;
                ValueLength = 0;
                IsEnd = true;
                return;
            }

            // TODO: Make the rest.
            throw new NotImplementedException();
        }
    }
}
