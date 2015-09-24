namespace Tesla.Net.FastCgi {
    public enum FastCgiRole
        : ushort {
        Responder = 1,
        Authorizer = 2,
        Filter = 3
    }
}