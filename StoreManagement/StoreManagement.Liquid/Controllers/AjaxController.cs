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

        public static Tuple<bool, String> GetCachingValue(String key)
        { 
            var returnHtml = (String) MemoryCache.Default.Get(key);
            return Tuple.Create(!String.IsNullOrEmpty(returnHtml), returnHtml);
        }

        public static void SetCachingValue(String key, String returnHtml, double dateTimeOffSetSeconds=0)
        {
            if (dateTimeOffSetSeconds == 0)
            {
                dateTimeOffSetSeconds = ProjectAppSettings.CacheMediumSeconds;
            }
            CacheItemPolicy policy = null;
            CacheEntryRemovedCallback callback = null;

            policy = new CacheItemPolicy();
            policy.Priority = CacheItemPriority.Default;
            policy.AbsoluteExpiration = DateTimeOffset.Now.AddSeconds(dateTimeOffSetSeconds);

            MemoryCache.Default.Set(key, returnHtml, policy);
        }
    
    }
}