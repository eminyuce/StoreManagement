using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using StoreManagement.Data;
using StoreManagement.Data.Entities;
using StoreManagement.Data.GeneralHelper;
using StoreManagement.Service.Interfaces;

namespace StoreManagement.Liquid.Helper
{
    public class StoreHelper 
    {
        public Store GetStoreByDomain(IStoreService storeService, HttpContextBase request)
        {
            String siteStatus = ProjectAppSettings.GetWebConfigString("SiteStatus", "dev");

            if (siteStatus.IndexOf("live", StringComparison.InvariantCultureIgnoreCase) >= 0)
            {

                String domainName = "FUELTECHNOLOGYAGE.COM";
                domainName = GeneralHelper.GetSiteDomain(request);
                return storeService.GetStore(domainName);
            }
            else
            {
                String defaultSiteDomain = ProjectAppSettings.GetWebConfigString("DefaultSiteDomain", "login.seatechnologyjobs.com");
                return storeService.GetStoreByDomain(defaultSiteDomain);
            }

        }
        public int GetStoreIdByDomain(IStoreService storeService, HttpContextBase request)
        {
            String siteStatus = ProjectAppSettings.GetWebConfigString("SiteStatus", "dev");

            if (siteStatus.IndexOf("live", StringComparison.InvariantCultureIgnoreCase) >= 0)
            {

                String domainName = "FUELTECHNOLOGYAGE.COM";
                domainName = GeneralHelper.GetSiteDomain(request);
                return storeService.GetStoreIdByDomain(domainName);
            }
            else
            {
                String defaultSiteDomain = ProjectAppSettings.GetWebConfigString("DefaultSiteDomain", "login.seatechnologyjobs.com");
                return storeService.GetStoreIdByDomain(defaultSiteDomain);
            }

        }
    }
}