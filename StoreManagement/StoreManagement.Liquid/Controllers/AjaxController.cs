using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Web;
using System.Web.Mvc;
using StoreManagement.Data;
using StoreManagement.Data.CacheHelper;

namespace StoreManagement.Liquid.Controllers
{
    public abstract class AjaxController : BaseController
    {

        protected Tuple<bool, String> GetCachingValue(String key)
        { 
            var returnHtml = (String) MemoryCache.Default.Get(key);
            return Tuple.Create(!String.IsNullOrEmpty(returnHtml), returnHtml);
        }

        protected void SetCachingValue(String key, String returnHtml)
        {
            CacheItemPolicy policy = null;
            CacheEntryRemovedCallback callback = null;

            policy = new CacheItemPolicy();
            policy.Priority = CacheItemPriority.Default;
            policy.AbsoluteExpiration = DateTimeOffset.Now.AddSeconds(ProjectAppSettings.CacheMediumSeconds);

            MemoryCache.Default.Set(key, returnHtml, policy);
        }
    
    }
}