using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StoreManagement.Data;
using StoreManagement.Data.CacheHelper;

namespace StoreManagement.Liquid.Controllers
{
    public abstract class AjaxController : BaseController
    {
        protected static readonly TypedObjectCache<String> AjaxRequestCache = new TypedObjectCache<String>("AjaxRequestCache");

        protected Tuple<bool, String> GetCachingValue(String key)
        {
            String returnHtml = "";
            AjaxRequestCache.TryGet(key, out returnHtml);
            return Tuple.Create(String.IsNullOrEmpty(returnHtml), returnHtml);
        }

        protected void SetCachingValue(String key, String returnHtml)
        {
            AjaxRequestCache.Set(key, returnHtml, MemoryCacheHelper.CacheAbsoluteExpirationPolicy(ProjectAppSettings.CacheMediumSeconds));
        }
    
    }
}