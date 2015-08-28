using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NLog;
using StoreManagement.Data;
using StoreManagement.Data.Entities;
using StoreManagement.Data.GeneralHelper;
using StoreManagement.Liquid.Helper.Interfaces;
using StoreManagement.Service.Interfaces;

namespace StoreManagement.Liquid.Helper
{
   
    public class StoreHelper : IStoreHelper
    {
        protected static readonly Logger Logger = LogManager.GetCurrentClassLogger();
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

            var sId=storeService.GetStoreIdByDomain(domainName);
            if (sId > 0)
            {
                return sId;
            }
            else
            {
                String defaultSiteDomain = ProjectAppSettings.GetWebConfigString("DefaultSiteDomain",
                                                                                "login.seatechnologyjobs.com");
                Logger.Trace("Default Site Domain is used." + defaultSiteDomain + " for " + domainName);
                return storeService.GetStoreIdByDomain(defaultSiteDomain);
            }
            

        }
    }
}