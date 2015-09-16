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
                return storeService.GetStore(domainName);
            }
            else
            {
                String defaultSiteDomain = ProjectAppSettings.GetWebConfigString("DefaultSiteDomain", "login.seatechnologyjobs.com");
                Logger.Trace("Default Site Domain is used." + defaultSiteDomain + " for " + domainName);
                return storeService.GetStoreByDomain(defaultSiteDomain);
            }

        }
        public int GetStoreIdByDomain(IStoreService storeService, HttpContextBase request)
        {
            String siteStatus = ProjectAppSettings.GetWebConfigString("SiteStatus", "dev");
            String domainName = "FUELTECHNOLOGYAGE.COM";

            domainName = GeneralHelper.GetSiteDomain(request);
            if (domainName.Contains("localhost"))
            {
                domainName = ProjectAppSettings.GetWebConfigString("DefaultSiteDomain",
                                                                               "login.seatechnologyjobs.com");
            }
            String key = domainName;
            int storeId = 0;
            if (siteStatus.IndexOf("live", StringComparison.InvariantCultureIgnoreCase) >= 0)
            {

                storeId = MemoryCache.Default.Get(key).ToInt();
                if (storeId == 0)
                {
                    storeId = storeService.GetStoreIdByDomain(domainName);

                    CacheItemPolicy policy = null;

                    policy = new CacheItemPolicy();
                    policy.Priority = CacheItemPriority.Default;
                    policy.AbsoluteExpiration = DateTimeOffset.Now.AddSeconds(ProjectAppSettings.CacheLongSeconds);

                    MemoryCache.Default.Set(key, storeId, policy);
                }

                return storeId;
            }
            else
            {
                storeId = storeService.GetStoreIdByDomain(domainName);
                return storeId;
            }
        }
    }
}