﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tesla.SocialApi.Vk
{
    public interface IVkApi
    {
        AuthorizationResult Authorize(string username, string password);
    }
}
