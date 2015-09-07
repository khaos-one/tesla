using Tesla;

namespace Tesla.SocialApi.Vk
{
    public enum AccessScope
        : uint
    {
        None            = 0,

        [EnumStringValue("notify")]
        Notify          = 1,

        [EnumStringValue("friends")]
        Friends         = 2,

        [EnumStringValue("photos")]
        Photos          = 4,

        [EnumStringValue("audio")]
        Audio           = 8,

        [EnumStringValue("video")]
        Video           = 16,

        [EnumStringValue("docs")]
        Docs            = 131072,

        [EnumStringValue("notes")]
        Notes           = 2048,

        [EnumStringValue("pages")]
        Pages           = 128,

        AddLink         = 256,

        [EnumStringValue("status")]
        Status          = 1024,

        [EnumStringValue("offers")]
        Offers          = 32,

        [EnumStringValue("questions")]
        Questions       = 64,

        [EnumStringValue("wall")]
        Wall            = 8192,

        [EnumStringValue("groups")]
        Groups          = 262144,

        [EnumStringValue("messages")]
        Messages        = 4096,

        [EnumStringValue("email")]
        Email           = 4194304,

        [EnumStringValue("notifications")]
        Notifications   = 524288,

        [EnumStringValue("stats")]
        Stats           = 1048576,

        [EnumStringValue("ads")]
        Ads             = 32768,

        [EnumStringValue("offline")]
        Offline         = 65536,

        [EnumStringValue("nohttps")]
        NoHttps
    }
}
