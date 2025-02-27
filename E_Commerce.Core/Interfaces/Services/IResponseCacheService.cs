﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Core.Interfaces.Services
{
    public interface IResponseCacheService
    {
        Task CacheResponseAsync(string Key, object Response, TimeSpan timeToLive);
        Task<string?> GetCachedResponseAsync(string Key);
    }
}
