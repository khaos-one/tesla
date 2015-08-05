namespace Tesla.Net.FastCgi
{
    /// <summary>
    /// Base record structure.
    /// <see cref="!:http://www.fastcgi.com/devkit/doc/fcgi-spec.html#S3.3"/>
    /// </summary>
    public class FastCgiRecord
    {
        public enum RecordType
        {
            BeginRequest = 1,
            AbortRequest = 2,
            EndRequest = 3,
            Params = 4,
            StdIn = 5,
            StdOut = 6,
            StdErr = 7,
            Data = 8,
            GetValues = 9,
            GetValuesResult = 10,
            Unknown = 11,

            BlankRecord = 99
        }

        public const int HeaderLength = 8;
        public byte Version;
        public RecordType Type;
        public ushort RequestId;
        public ushort ContentLength;
        public byte PaddingLength;
        public byte Reserved;
        public byte[] ContentData;
        public byte[] PaddingData;
    }
}
