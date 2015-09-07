using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tesla.SocialApi.Vk
{
    public sealed class VkApi
        : IVkApi
    {
        public const string ApiVersion = "5.34";

        public ulong AppId { get; private set; }
        public ulong UserId { get; private set; }
        public AccessScope Scope { get; private set; }
        public string AccessToken { get; private set; }

        public VkApi(ulong appId, AccessScope scope)
        {
            AppId = appId;
            Scope = scope;
        }
    }
}
