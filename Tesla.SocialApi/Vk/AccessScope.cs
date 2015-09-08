using System;

namespace Tesla.SocialApi.Vk
{
    [Flags]
    public enum AccessScope
        : uint
    {
        None            = 0,
        Notify          = 1,
        Friends         = 2,
        Photos          = 4,
        Audio           = 8,
        Video           = 16,
        Docs            = 131072,
        Notes           = 2048,
        Pages           = 128,
        AddLink         = 256,
        Status          = 1024,
        Offers          = 32,
        Questions       = 64,
        Wall            = 8192,
        Groups          = 262144,
        Messages        = 4096,
        Email           = 4194304,
        Notifications   = 524288,
        Stats           = 1048576,
        Ads             = 32768,
        Offline         = 65536,
        NoHttps         = 2147483648
    }

    public static class AccessScopeExtensions
    {
        public static string GetUrlString(this AccessScope scope)
        {
            return scope.ToString().Replace(" ", "").ToLowerInvariant();
        }

        public static AccessScope All
        {
            get
            {
                return AccessScope.Notify | AccessScope.Friends | AccessScope.Photos |
                       AccessScope.Audio | AccessScope.Video | AccessScope.Docs |
                       AccessScope.Notes | AccessScope.Pages | AccessScope.Status |
                       AccessScope.Offers | AccessScope.Questions | AccessScope.Wall |
                       AccessScope.Groups | AccessScope.Messages | AccessScope.Email |
                       AccessScope.Notifications | AccessScope.Stats | AccessScope.Ads |
                       AccessScope.Offline;
            }
        }

        public static AccessScope AllWithNoHttps
        {
            get
            {
                return All | AccessScope.NoHttps;
            }
        }
    }
}
