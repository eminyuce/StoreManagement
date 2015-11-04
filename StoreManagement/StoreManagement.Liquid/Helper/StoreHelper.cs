using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Threading.Tasks;
using System.Web;
using NLog;
using StoreManagement.Data;
using StoreManagement.Data.CacheHelper;
using StoreManagement.Data.Entities;
using StoreManagement.Data.GeneralHelper;
using StoreManagement.Data.Paging;
using StoreManagement.Liquid.Helper.Interfaces;
using StoreManagement.Service.Interfaces;

namespace StoreManagement.Liquid.Helper
{

    public class StoreHelper : IStoreHelper
    {
        protected static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private static readonly TypedObjectCache<Store>
            StoreCache = new TypedObjectCache<Store>("StoreHelper");

        public Store GetStoreByDomain(IStoreService storeService, HttpContextBase request)
        {
            String siteStatus = ProjectAppSettings.GetWebConfigString("SiteStatus", "dev");
            String domainName = "FUELTECHNOLOGYAGE.COM";
            domainName = GeneralHelper.GetSiteDomain(request);
            if (siteStatus.IndexOf("live", StringComparison.InvariantCultureIgnoreCase) >= 0)
            {
                String key = domainName;
                Store storeObj = new Store();
                storeObj = (Store)MemoryCache.Default.Get(key);
                if (storeObj == null)
                {
                    storeObj = storeService.GetStoreByDomain(domainName);

                    CacheItemPolicy policy = null;

                    policy = new CacheItemPolicy();
                    policy.Priority = CacheItemPriority.Default;
                    policy.AbsoluteExpiration = DateTimeOffset.Now.AddSeconds(ProjectAppSettings.CacheLongSeconds);

                    MemoryCache.Default.Set(key, storeObj, policy);
                }
                return storeObj;

            }
            else
            {
                String defaultSiteDomain = ProjectAppSettings.GetWebConfigString("DefaultSiteDomain", "login.seatechnologyjobs.com");
                Logger.Trace("Default Site Domain is used." + defaultSiteDomain + " for " + domainName);
                return storeService.GetStoreByDomain(defaultSiteDomain);
            }

        }
       
    }
}