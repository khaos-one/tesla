namespace Tesla.Protocol.Base0
{
    public class BaseRequest
    {
        public ushort ProtocolVersion { get; set; }
        public string Function { get; set; }
    }
}
