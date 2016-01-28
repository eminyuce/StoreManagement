using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Web;
using System.Web.Mvc;
using StoreManagement.Data;
using StoreManagement.Data.Constants;
using StoreManagement.Data.Entities;
using StoreManagement.Data.GeneralHelper;
using StoreManagement.Data.RequestModel;


namespace StoreManagement.Controllers
{
    public abstract class AjaxController : BaseController
    {

        public ActionResult Refresh(String domain)
        {
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        public static Tuple<bool, String> GetCachingValue(String key)
        {
            var returnHtml = (String)MemoryCache.Default.Get(key);
            return Tuple.Create(!String.IsNullOrEmpty(returnHtml), returnHtml);
        }

        public static void SetCachingValue(String key, String returnHtml, double dateTimeOffSetSeconds = 0)
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